using System.Linq.Expressions;

namespace TalentFlow.Domain.Abstractions.Specifications;

/// <summary>
/// Интерфейс для построения спецификаций сущностей.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public interface ISpecificationBuilder<T> where T : class
{
    /// <summary>
    /// Добавляет критерий фильтрации к спецификации.
    /// </summary>
    /// <param name="predicate">Выражение фильтрации.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Добавляет включение навигационного свойства (Include).
    /// </summary>
    /// <typeparam name="TProperty">Тип включаемого свойства.</typeparam>
    /// <param name="include">Выражение для включения навигационного свойства.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> include);

    /// <summary>
    /// Добавляет последующее включение (ThenInclude) для цепочки навигационных свойств.
    /// </summary>
    /// <typeparam name="TPreviousProperty">Тип предыдущего навигационного свойства.</typeparam>
    /// <typeparam name="TProperty">Тип включаемого свойства.</typeparam>
    /// <param name="thenInclude">Выражение для последующего включения.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> ThenInclude<TPreviousProperty, TProperty>(
        Expression<Func<TPreviousProperty, TProperty>> thenInclude);

    /// <summary>
    /// Добавляет сортировку по возрастанию для указанного ключа.
    /// </summary>
    /// <param name="keySelector">Выражение для выбора ключа сортировки.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> OrderBy(Expression<Func<T, object>> keySelector);

    /// <summary>
    /// Добавляет сортировку по убыванию для указанного ключа.
    /// </summary>
    /// <param name="keySelector">Выражение для выбора ключа сортировки.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> keySelector);

    /// <summary>
    /// Устанавливает количество пропускаемых элементов для пагинации.
    /// </summary>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> Skip(int skip);

    /// <summary>
    /// Устанавливает количество выбираемых элементов для пагинации.
    /// </summary>
    /// <param name="take">Количество элементов для выборки.</param>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> Take(int take);

    /// <summary>
    /// Указывает, что запрос должен выполняться без отслеживания изменений.
    /// </summary>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> AsNoTracking();

    /// <summary>
    /// Указывает, что запрос должен использовать разделенный запрос (Split Query).
    /// </summary>
    /// <returns>Текущий билдер для цепочки вызовов.</returns>
    ISpecificationBuilder<T> UseSplitQuery();

    /// <summary>
    /// Создает спецификацию на основе текущих настроек.
    /// </summary>
    /// <returns>Готовая спецификация.</returns>
    ISpecification<T> Build();
}