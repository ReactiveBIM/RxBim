namespace PikTools.Shared.AutocadExtensions.Extensions.CoordinateSystems
{
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    using JetBrains.Annotations;
    using AcRx = Autodesk.AutoCAD.Runtime;

    /// <summary>
    /// Расширения для редактора для работы с системами координат
    /// </summary>
    [PublicAPI]
    public static class EditorCoordinateSystemExtensions
    {
        /// <summary>
        /// Transforms a point from a coordinate system to another one in the specified editor.
        /// </summary>
        /// /// <param name="ed">An instance of the Editor to which the method applies.</param>
        /// <param name="pt">The instance to which the method applies.</param>
        /// <param name="from">The origin coordinate system.</param>
        /// <param name="to">The target coordinate system.</param>
        /// <returns>The corresponding 3d point.</returns>
        /// <exception cref="Autodesk.AutoCAD.Runtime.Exception">
        /// eInvalidInput is thrown if CoordSystem.PSDCS is used with other than CoordSystem.DCS.</exception>
        public static Point3d TransformPoint(
            this Editor ed,
            Point3d pt,
            CoordinateSystemType from,
            CoordinateSystemType to)
        {
#pragma warning disable SA1129
            var mat = new Matrix3d();
#pragma warning restore SA1129
            switch (from)
            {
                case CoordinateSystemType.WCS:
                    switch (to)
                    {
                        case CoordinateSystemType.UCS:
                            mat = ed.GetTransformMatrixFromWCSToUCS();
                            break;

                        case CoordinateSystemType.DCS:
                            mat = ed.GetTransformMatrixFromWCSToDCS();
                            break;

                        case CoordinateSystemType.PSDCS:
                            throw new AcRx.Exception(AcRx.ErrorStatus.InvalidInput, "To be used only with DCS");

                        default:
                            mat = Matrix3d.Identity;
                            break;
                    }

                    break;

                case CoordinateSystemType.UCS:
                    switch (to)
                    {
                        case CoordinateSystemType.WCS:
                            mat = ed.GetTransformMatrixFromUCSToWCS();
                            break;

                        case CoordinateSystemType.DCS:
                            mat = ed.GetTransformMatrixFromUCSToWCS() * ed.GetTransformMatrixFromWCSToDCS();
                            break;

                        case CoordinateSystemType.PSDCS:
                            throw new AcRx.Exception(AcRx.ErrorStatus.InvalidInput, "To be used only with DCS");

                        default:
                            mat = Matrix3d.Identity;
                            break;
                    }

                    break;

                case CoordinateSystemType.DCS:
                    switch (to)
                    {
                        case CoordinateSystemType.WCS:
                            mat = ed.GetTransformMatrixFromDCSToWCS();
                            break;

                        case CoordinateSystemType.UCS:
                            mat = ed.GetTransformMatrixFromDCSToWCS() * ed.GetTransformMatrixFromWCSToUCS();
                            break;

                        case CoordinateSystemType.PSDCS:
                            mat = ed.GetTransformMatrixFromDCSToPSDCS();
                            break;

                        default:
                            mat = Matrix3d.Identity;
                            break;
                    }

                    break;

                case CoordinateSystemType.PSDCS:
                    switch (to)
                    {
                        case CoordinateSystemType.WCS:
                            throw new AcRx.Exception(AcRx.ErrorStatus.InvalidInput, "To be used only with DCS");
                        case CoordinateSystemType.UCS:
                            throw new AcRx.Exception(AcRx.ErrorStatus.InvalidInput, "To be used only with DCS");
                        case CoordinateSystemType.DCS:
                            mat = ed.GetTransformMatrixFromPSDCSToDCS();
                            break;

                        default:
                            mat = Matrix3d.Identity;
                            break;
                    }

                    break;
            }

            return pt.TransformBy(mat);
        }

        /// <summary>
        /// Gets the transformation matrix from the paper space active viewport Display Coordinate System (DCS)
        /// to the Paper space Display Coordinate System (PSDCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The DCS to PSDCS transformation matrix.</returns>
        /// <exception cref="Autodesk.AutoCAD.Runtime.Exception">
        /// eNotInPaperSpace is thrown if this method is called form Model Space.</exception>
        /// <exception cref="Autodesk.AutoCAD.Runtime.Exception">
        /// eCannotChangeActiveViewport is thrown if there is none floating viewport in the current layout.</exception>
        public static Matrix3d GetTransformMatrixFromDCSToPSDCS(this Editor ed)
        {
            var db = ed.Document.Database;
            if (db.TileMode)
            {
                throw new AcRx.Exception(AcRx.ErrorStatus.NotInPaperspace);
            }

            using var tr = db.TransactionManager.StartTransaction();
            var vp = (Viewport)tr.GetObject(ed.CurrentViewportObjectId, OpenMode.ForRead);
            if (vp.Number == 1)
            {
                try
                {
                    ed.SwitchToModelSpace();
                    vp = (Viewport)tr.GetObject(ed.CurrentViewportObjectId, OpenMode.ForRead);
                    ed.SwitchToPaperSpace();
                }
                catch
                {
                    throw new AcRx.Exception(AcRx.ErrorStatus.CannotChangeActiveViewport);
                }
            }

            return vp.GetTransformMatrixFromDCSToPSDCS();
        }

        /// <summary>
        /// Gets the transformation matrix from the current viewport Display Coordinate System (DCS)
        /// to the World Coordinate System (WCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The DCS to WCS transformation matrix.</returns>
        public static Matrix3d GetTransformMatrixFromDCSToWCS(this Editor ed)
        {
            Matrix3d retVal;
            var tileMode = ed.Document.Database.TileMode;
            if (!tileMode)
                ed.SwitchToModelSpace();
            using (var vtr = ed.GetCurrentView())
            {
                retVal =
                    Matrix3d.Rotation(-vtr.ViewTwist, vtr.ViewDirection, vtr.Target) *
                    Matrix3d.Displacement(vtr.Target - Point3d.Origin) *
                    Matrix3d.PlaneToWorld(vtr.ViewDirection);
            }

            if (!tileMode)
                ed.SwitchToPaperSpace();
            return retVal;
        }

        /// <summary>
        /// Gets the transformation matrix from the Paper space Display Coordinate System (PSDCS)
        /// to the paper space active viewport Display Coordinate System (DCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The PSDCS to DCS transformation matrix.</returns>
        /// <exception cref=" Autodesk.AutoCAD.Runtime.Exception">
        /// eNotInPaperSpace is thrown if this method is called form Model Space.</exception>
        /// <exception cref=" Autodesk.AutoCAD.Runtime.Exception">
        /// eCannotChangeActiveViewport is thrown if there is none floating viewport in the current layout.</exception>
        public static Matrix3d GetTransformMatrixFromPSDCSToDCS(this Editor ed)
        {
            return ed.GetTransformMatrixFromDCSToPSDCS().Inverse();
        }

        /// <summary>
        /// Gets the transformation matrix from the current User Coordinate System (UCS)
        /// to the World Coordinate System (WCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The UCS to WCS transformation matrix.</returns>
        public static Matrix3d GetTransformMatrixFromUCSToWCS(this Editor ed)
        {
            return ed.CurrentUserCoordinateSystem;
        }

        /// <summary>
        /// Gets the transformation matrix from the World Coordinate System (WCS)
        /// to the current viewport Display Coordinate System (DCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The WCS to DCS transformation matrix.</returns>
        public static Matrix3d GetTransformMatrixFromWCSToDCS(this Editor ed)
        {
            return ed.GetTransformMatrixFromDCSToWCS().Inverse();
        }

        /// <summary>
        /// Gets the transformation matrix from the World Coordinate System (WCS)
        /// to the current User Coordinate System (UCS).
        /// </summary>
        /// <param name="ed">The instance to which this method applies.</param>
        /// <returns>The WCS to UCS transformation matrix.</returns>
        public static Matrix3d GetTransformMatrixFromWCSToUCS(this Editor ed)
        {
            return ed.CurrentUserCoordinateSystem.Inverse();
        }
    }
}