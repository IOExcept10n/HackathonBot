
namespace MyBots.Core.Fsm;

public interface IStatsCollector
{
    /// <summary>
    /// Увеличить счётчик активных сессий (например, при создании новой сессии).
    /// </summary>
    /// <param name="userId">Telegram user id сессии.</param>
    void IncrementActiveSessions(long userId);

    /// <summary>
    /// Уменьшить счётчик активных сессий (например, при удалении/таймауте сессии).
    /// </summary>
    /// <param name="userId">Telegram user id сессии.</param>
    void DecrementActiveSessions(long userId);

    /// <summary>
    /// Зарегистрировать наблюдаемую длительность пребывания в состоянии.
    /// Вызывается при выходе пользователя из состояния (или при переходе).
    /// </summary>
    /// <param name="userId">Telegram user id.</param>
    /// <param name="stateId">Идентификатор состояния (например, "module:flow:step").</param>
    /// <param name="duration">Время, проведённое в состоянии.</param>
    void ObserveStateDuration(long userId, string stateId, TimeSpan duration);

    /// <summary>
    /// Зарегистрировать время ответа пользователя на действие/вопрос в состоянии.
    /// (опционально: среднее время ответа, полезно для промптов)
    /// </summary>
    /// <param name="userId">Telegram user id.</param>
    /// <param name="stateId">Идентификатор состояния.</param>
    /// <param name="responseTime">Время от показа сообщения до получения ответа.</param>
    void ObserveUserResponseTime(long userId, string stateId, TimeSpan responseTime);

    /// <summary>
    /// Инкрементировать счётчик кастомного события (например, "prompts_registered", "submissions_uploaded").
    /// </summary>
    /// <param name="name">Имя метрики.</param>
    /// <param name="tags">Необязательные теги/пары ключ-значение для идентификации (module, role и т.д.).</param>
    void IncrementEvent(string name, IDictionary<string, string>? tags = null);

    /// <summary>
    /// Записать произвольную числовую метрику (гистограмма, гейдж или значение).
    /// </summary>
    /// <param name="name">Имя метрики.</param>
    /// <param name="value">Числовое значение.</param>
    /// <param name="tags">Опциональные теги.</param>
    void RecordMetric(string name, double value, IDictionary<string, string>? tags = null);
}