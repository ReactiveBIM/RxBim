namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Architecture;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// Расширения для помещений
    /// </summary>
    public static class RoomExtensions
    {
        /// <summary>
        /// Проверка помещения, что оно не размещено или не окружено или имеет избыточную площать
        /// </summary>
        /// <param name="room">Помещение</param>
        public static Result IsAreaValid(this Room room)
        {
            return Result.SuccessIf(
                    room != null,
                    "Не задано помещение для проверки")
                .Ensure(
                    () => !(Math.Abs(room.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble()) < 0.001),
                    "Помещение возможно не размещено или не окружено или имеет избыточную площать");
        }
    }
}
