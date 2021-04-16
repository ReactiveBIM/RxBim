namespace TableBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using PikTools.Shared.TableBuilder;
    using PikTools.Shared.TableBuilder.Extensions;
    using Xunit;
    using Color = System.Drawing.Color;

    /// <summary>
    /// Тесты для <see cref="Table"/>
    /// </summary>
    public class TableBuildingTests
    {
        /// <summary> Тест заполнения таблицы по строкам </summary>
        [Fact]
        public void FillRowsTest()
        {
            var assert = 10;

            var data = GetTestData(assert);

            var result = new Table(100)
                .AddRow(x => x.FromList(data, 0, 0, p => p.Prop1, p => p.Prop2));

            result.Rows.Count.Should().Be(assert);
            result.Columns.Count.Should().Be(2);
        }

        /// <summary> Тест заполнения таблицы по столбцам </summary>
        [Fact]
        public void FillCellsTest()
        {
            var assert = 10;

            var data = GetTestData(assert);

            var result = new Table(100)
                .AddColumn(x => x.FromList(data, 0, 0, p => p.Prop1, p => p.Prop2));

            result.Rows.Count.Should().Be(2);
            result.Columns.Count.Should().Be(assert);
        }

        /// <summary> Тест получения элемента по индексу </summary>
        [Fact]
        public void GetByIndexTest()
        {
            var assert = 10;

            var data = GetTestData(assert);

            var table = new Table(100)
                .AddRow(x => x.FromList(data, 1, 1, p => p.Prop1, p => p.Prop2));

            GetAssertedCellData(table[0, 0]).Value.Should().BeEquivalentTo(string.Empty);
            GetAssertedCellData(table[0, 0]).Value.Should().BeEquivalentTo(string.Empty);
            GetAssertedCellData(table[0, 0]).Value.Should().BeEquivalentTo(string.Empty);

            table.Columns[0].Cells
                .Select(x => ((TextCellData)x.Data).Value)
                .Should()
                .OnlyContain(x => x == string.Empty);

            table.Rows[0].Cells
                .Select(x => ((TextCellData)x.Data).Value)
                .Should()
                .OnlyContain(x => x == string.Empty);

            for (var r = 1; r <= assert; r++)
            {
                var cell = table[r, 1];
                var cellData = GetAssertedCellData(cell);
                var srcData = data[r - 1];

                cellData.Value.Should().BeEquivalentTo(srcData.Prop1.ToString());

                var rightCell = cell.Next();
                cellData = GetAssertedCellData(rightCell);

                cellData.Value.Should().BeEquivalentTo(srcData.Prop2);
            }
        }

        /// <summary> Тест назначения стилей </summary>
        [Fact]
        public void CellsStyleTest()
        {
            var assert = 5;
            var columnWidth = 85;
            var columnHeight = 95;
            var data = GetTestData(assert);
            var borders = new CellBorders(left: CellBorderType.Hidden, bottom: CellBorderType.Hidden);
            var format = new CellFormatStyle(
                bold: true,
                italic: false,
                borders: borders,
                textColor: Color.Blue,
                backgroundColor: Color.Bisque,
                textSize: 55,
                textHorizontalAlignment: TextHorizontalAlignment.Left,
                textVerticalAlignment: TextVerticalAlignment.Top);

            var table = new Table(100)
                .AddColumn(x => x.SetFormat(format).SetWidth(columnWidth))
                .AddColumn(count: data.Count)
                .AddRow(x => x.FromList(data).SetHeight(columnHeight));

            table.Columns[0].DefaultCellFormat.Should().BeEquivalentTo(format);
            table.Columns[0].Width.Should().Be(columnWidth);
            table.Columns[0].Cells.ForEach(x => x.Format.Should().BeEquivalentTo(format));
            table.Rows[0].Height.Should().Be(columnHeight);
        }

        /// <summary> Тест назначения стилей для всей таблицы </summary>
        [Fact]
        public void TableStyleTest()
        {
            var assert = 5;
            var columnWidth = 85;
            var columnHeight = 95;
            var data = GetTestData(assert);
            var borders = new CellBorders(left: CellBorderType.Hidden, bottom: CellBorderType.Hidden);
            var format = new CellFormatStyle(
                bold: true,
                italic: false,
                borders: borders,
                textColor: Color.Blue,
                backgroundColor: Color.Bisque,
                textSize: 55,
                textHorizontalAlignment: TextHorizontalAlignment.Left,
                textVerticalAlignment: TextVerticalAlignment.Top);

            var table = new Table()
                .AddColumn(x => x.SetWidth(columnWidth))
                .AddColumn(x => x.SetWidth(columnWidth), count: data.Count)
                .AddRow(x => x.FromList(data).SetHeight(columnHeight));

            table.SetFormat(format);

            table.Columns.Select(e => e.DefaultCellFormat).Should().AllBeEquivalentTo(format);
            table.Columns.Select(e => e.Width).Should().AllBeEquivalentTo(columnWidth);
            table.Columns.SelectMany(e => e.Cells.Select(x => x.Format)).Should().AllBeEquivalentTo(format);
            table.Columns[0].Cells.ForEach(x => x.Format.Should().BeEquivalentTo(format));
            table.Rows.Select(e => e.Height).Should().AllBeEquivalentTo(columnHeight);
        }

        /// <summary> Тест назначения стилей для всей таблицы </summary>
        [Fact]
        public void TableRangeStyleTest()
        {
            var assert = 5;
            var columnWidth = 85;
            var columnHeight = 95;
            var data = GetTestData(assert);
            var borders = new CellBorders(left: CellBorderType.Hidden, bottom: CellBorderType.Hidden);
            var format = new CellFormatStyle(
                bold: true,
                italic: false,
                borders: borders,
                textColor: Color.Blue,
                backgroundColor: Color.Bisque,
                textSize: 55,
                textHorizontalAlignment: TextHorizontalAlignment.Left,
                textVerticalAlignment: TextVerticalAlignment.Top);

            var table = new Table()
                .AddColumn(x => x.SetWidth(columnWidth))
                .AddColumn(x => x.SetWidth(columnWidth), count: data.Count)
                .AddRow(x => x.FromList(data).SetHeight(columnHeight));

            table.SetFormat(format, 0, 1, 2, 1);

            table.Columns.Skip(1).Take(2).SelectMany(x => x.Cells.Select(e => e.Format)).Should().AllBeEquivalentTo(format);
            table.Columns[0].Cells.ForEach(x => x.Format.Should().NotBeEquivalentTo(format));
            table.Columns[3].Cells.ForEach(x => x.Format.Should().NotBeEquivalentTo(format));
            table.Columns.ForEach(x => x.DefaultCellFormat.Should().NotBeEquivalentTo(format));
            table.Columns.Select(e => e.Width).Should().AllBeEquivalentTo(columnWidth);
            table.Rows.Select(e => e.Height).Should().AllBeEquivalentTo(columnHeight);
        }

        /// <summary> Тест объединения ячеек </summary>
        [Fact]
        public void CellMergeTest()
        {
            var data = GetTestData(10);

            var table = new Table(100)
                .AddColumn(x => x.FromList(data, 0, 0, p => p.Prop1, p => p.Prop2))
                .AddColumn(x => x.FromList(data, 2, 0, p => p.Prop1, p => p.Prop2))
                .AddColumn(x => x.FromList(data, 4, 0, p => p.Prop1, p => p.Prop2))[1, 3]
                .MergeNext(1, action: (x, y) => { }).And[1, 3].MergeDown(1, action: (x, y) => { })
                .And.Build();

            table[1, 3].Merged.Should().BeTrue();
            table[1, 4].Merged.Should().BeTrue();
            table[2, 3].Merged.Should().BeTrue();
            table[2, 4].Merged.Should().BeTrue(); // Т.к объединение возможно только в прямоугольник

            var mergedArea = table[2, 4].Area;
            mergedArea.TopRow.Should().Be(1);
            mergedArea.BottomRow.Should().Be(2);
            mergedArea.LeftColumn.Should().Be(3);
            mergedArea.RightColumn.Should().Be(4);
        }

        /// <summary> Тест задает данные объединенной ячейке и они прописываются всем ячейкам в объединении </summary>
        [Fact]
        public void SetDataToMeregedCell()
        {
            var colCount = 4;
            var rowCount = 4;
            var cellValue = new TextCellData("test");

            var table = new Table()
                .AddColumn(count: colCount).AddRow(count: rowCount)
                .Rows.First().Cells.First()
                .MergeNext(colCount - 1).MergeDown(rowCount - 1)
                .SetValue(cellValue)
                .And.Build();

            for (var i = 0; i < colCount; i++)
            {
                for (var j = 0; j < rowCount; j++)
                    table[j, i].Data.Should().Be(cellValue);
            }
        }

        /// <summary> Тест задает формат объединенной ячейке и они прописываются всем ячейкам в объединении </summary>
        [Fact]
        public void SetFormatToMeregedCell()
        {
            var colCount = 4;
            var rowCount = 4;
            var format = new CellFormatStyle(bold: true);

            var table = new Table()
                .AddColumn(count: colCount).AddRow(count: rowCount)
                .Rows.First().Cells.First()
                .MergeNext(colCount - 1).MergeDown(rowCount - 1)
                .SetFormat(format)
                .And.Build();

            for (var i = 0; i < colCount; i++)
            {
                for (var j = 0; j < rowCount; j++)
                    table[j, i].Format.Should().Be(format);
            }
        }

        /// <summary> Тест при объединении ячеек выдается последняя ячейка в объединении </summary>
        [Fact]
        public void MergeCellReturnLastCell()
        {
            var colCount = 4;
            var rowCount = 4;

            var table = new Table()
                .AddColumn(count: colCount).AddRow(count: rowCount);

            var nextMergedCell = table.Rows.First().Cells[0].MergeNext();
            nextMergedCell.Number.Should().Be(1);

            var downMergedCell = table.Columns.Last().Cells[0].MergeDown();
            downMergedCell.Row.Number.Should().Be(table.Rows[1].Number);

            var leftMergedCell = table.Rows.Last().Cells[1].MergeLeft();
            leftMergedCell.Number.Should().Be(0);
        }

        private List<Example> GetTestData(int count) =>
            Enumerable.Range(0, count)
                .Select(x => new Example { Prop1 = x, Prop2 = nameof(TableBuildingTests) + x })
                .ToList();

        private TextCellData GetAssertedCellData(Cell cell)
        {
            cell.Should().NotBeNull();
            return cell.Data as TextCellData;
        }

        private class Example
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
        }
    }
}