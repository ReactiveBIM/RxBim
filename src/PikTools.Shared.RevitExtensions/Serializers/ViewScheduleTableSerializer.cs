namespace PikTools.Shared.RevitExtensions.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using PikTools.Shared.RevitExtensions.Extensions;
    using PikTools.Shared.TableBuilder;
    using PikTools.Shared.TableBuilder.Abstractions;

    /// <inheritdoc />
    public class ViewScheduleTableSerializer<T> : ITableSerializer<ViewSchedule>
        where T : ViewSchedule
    {
        private const double FontRatio = 3.77951502;
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewScheduleTableSerializer{T}"/> class.
        /// </summary>
        /// <param name="document">документ.</param>
        public ViewScheduleTableSerializer(Document document)
        {
            _document = document;
        }

        /// <inheritdoc />
        public ViewSchedule Serialize(Table table, ITableSerializerParameters @params)
        {
            var parameters = @params as ViewScheduleTableSerializerParameters
                ?? throw new Exception("Параметры сериализации должны быть заданы");

            using var t = new Transaction(_document);
            t.Start(nameof(ViewScheduleTableSerializer<T>));

            var id = new ElementId((int)BuiltInCategory.OST_NurseCallDevices);

            var schedule = ViewSchedule.CreateSchedule(_document, id);
            schedule.Name = parameters.Name;
            schedule.Definition.ShowHeaders = false;

            var field = schedule.Definition
                .GetSchedulableFields()
                .FirstOrDefault(x =>
                    x.GetName(_document).ToUpper() == "URL");

            if (field != null)
            {
                schedule.Definition.AddField(field).GridColumnWidth = table.Width.HasValue ?
                    table.Width.Value.MmToFt() :
                    table.Rows.Select(roe => roe.Width).Max().MmToFt();
            }

            var tableData = schedule.GetTableData();
            var headerData = tableData.GetSectionData(SectionType.Header);

            InsertCells(headerData, table.Rows.Count - 1, table.Columns.Count);

            var mergedCells = new List<TableMergedArea>();

            var scheduleCol = headerData.FirstColumnNumber;

            for (var col = 0;
                col < table.Columns.Count;
                col++, scheduleCol++)
            {
                var widthInFt = table[0, col].Width.MmToFt();
                headerData.SetColumnWidth(scheduleCol, widthInFt);

                var scheduleRow = headerData.FirstRowNumber;

                for (var row = 0;
                    row < table.Rows.Count;
                    row++, scheduleRow++)
                {
                    var cell = table[row, col];

                    var rowHeight = table.Rows[row].Height;
                    rowHeight = rowHeight > 0 ? rowHeight.MmToFt() : 8.MmToFt();

                    var text = cell.Data as TextCellData;
                    headerData.SetRowHeight(scheduleRow, rowHeight);
                    headerData.SetCellText(scheduleRow, scheduleCol, text?.Value);
                    headerData.SetCellStyle(scheduleRow, scheduleCol, GetCellStyle(cell.Format, parameters));

                    // Merge logic
                    if (!cell.Merged || mergedCells.Exists(x => Equals(x, cell.Area)))
                        continue;

                    mergedCells.Add(cell.Area);

                    var tableMergedCell = new TableMergedCell
                    {
                        Bottom = cell.Area.BottomRow,
                        Top = cell.Area.TopRow,
                        Left = cell.Area.LeftColumn,
                        Right = cell.Area.RightColumn
                    };

                    headerData.MergeCells(tableMergedCell);
                    EnsureTableMergedCell(tableMergedCell, scheduleRow, scheduleCol);
                }
            }

            headerData.RemoveColumn(headerData.LastColumnNumber);

            t.Commit();
            return schedule;
        }

        private TableCellStyle GetCellStyle(
            CellFormatStyle cellStyle,
            ViewScheduleTableSerializerParameters parameters)
        {
            var options = new TableCellStyleOverrideOptions
            {
                BorderTopLineStyle = cellStyle.Borders.Top != CellBorderType.Usual,
                BorderBottomLineStyle = cellStyle.Borders.Bottom != CellBorderType.Usual,
                BorderLeftLineStyle = cellStyle.Borders.Left != CellBorderType.Usual,
                BorderRightLineStyle = cellStyle.Borders.Right != CellBorderType.Usual,
                FontSize = cellStyle.TextSize > 0,
                Bold = true,
                Italics = true,
                HorizontalAlignment = true,
                VerticalAlignment = true,
                FontColor = true,
                BackgroundColor = true
            };

            var boldLineId = parameters.SpecificationBoldLineId ?? -1;

            var style = new TableCellStyle
            {
                BorderTopLineStyle = GetLineId(cellStyle.Borders.Top, boldLineId),
                BorderBottomLineStyle = GetLineId(cellStyle.Borders.Bottom, boldLineId),
                BorderLeftLineStyle = GetLineId(cellStyle.Borders.Left, boldLineId),
                BorderRightLineStyle = GetLineId(cellStyle.Borders.Right, boldLineId),
                TextSize = cellStyle.TextSize * FontRatio,
                IsFontBold = cellStyle.Bold,
                IsFontItalic = cellStyle.Italic,
                TextColor = GetRevitColor(cellStyle.TextColor),
                BackgroundColor = GetRevitColor(cellStyle.BackgroundColor),
                FontVerticalAlignment = cellStyle.TextVerticalAlignment switch
                {
                    TextVerticalAlignment.Top => VerticalAlignmentStyle.Top,
                    TextVerticalAlignment.Middle => VerticalAlignmentStyle.Middle,
                    _ => VerticalAlignmentStyle.Bottom
                },
                FontHorizontalAlignment = cellStyle.TextHorizontalAlignment switch
                {
                    TextHorizontalAlignment.Right => HorizontalAlignmentStyle.Right,
                    TextHorizontalAlignment.Left => HorizontalAlignmentStyle.Left,
                    _ => HorizontalAlignmentStyle.Center
                }
            };

            style.SetCellStyleOverrideOptions(options);

            return style;
        }

        private ElementId GetLineId(CellBorderType borderType, int boldLineId = -1)
        {
            return borderType switch
            {
                CellBorderType.Usual => ElementId.InvalidElementId, // не применяется
                CellBorderType.Hidden => ElementId.InvalidElementId,
                CellBorderType.Bold => new ElementId(boldLineId),
                _ => throw new Exception($"Линия {borderType} не реализована в сериализаторе"),
            };
        }

        private void InsertCells(TableSectionData section, int rows, int cols)
        {
            for (var j = 0; j < rows; j++)
                section.InsertRow(j);

            for (var j = 0; j < cols; j++)
                section.InsertColumn(j);
        }

        private void EnsureTableMergedCell(TableMergedCell tableMergedCell, int row, int col)
        {
            if (tableMergedCell.Bottom < row)
                tableMergedCell.Bottom = row;

            if (tableMergedCell.Top > row)
                tableMergedCell.Top = row;

            if (tableMergedCell.Left > col)
                tableMergedCell.Left = col;

            if (tableMergedCell.Right < col)
                tableMergedCell.Right = col;
        }

        private Color GetRevitColor(System.Drawing.Color color) =>
            new Color(red: color.R, green: color.G, blue: color.B);
    }
}