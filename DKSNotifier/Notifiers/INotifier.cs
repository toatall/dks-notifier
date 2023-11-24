namespace DKSNotifier.Notifiers
{
    /// <summary>
    /// Уведомление
    /// </summary>
    internal interface INotifier
    {
        /// <summary>
        /// Запуск процедуры уведомления
        /// </summary>
        /// <param name="text">текст уведомления</param>
        void Exec(string text);
    }
}
