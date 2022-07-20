// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using FluentAssertions;
// using FluentAssertions.Collections;
// using FluentAssertions.Equivalency;
// using JetBrains.Annotations;
// using SKBKontur.Billy.Core.TestingCore.Assertions.FluentAssertionRules;
//
// namespace SKBKontur.Billy.Core.TestingCore.Assertions
// {
//     public static class CollectionAssertionsExtensions
//     {
//         public static void BeEquivalentByReferencesTo<TSubject, TAssertions>(
//             [NotNull] this CollectionAssertions<TSubject, TAssertions> collectionAssertions,
//             IEnumerable expectation,
//             string because = "",
//             params object[] becauseArgs)
//             where TSubject : IEnumerable
//             where TAssertions : CollectionAssertions<TSubject, TAssertions>
//         {
//             var subjectSet = new H
//             var 
//         }
//
//         public static void BeEquivalentByReferencesTo<TSubject, TExpected, TAssertions>(
//             [NotNull] this CollectionAssertions<TSubject, TAssertions> collectionAssertions,
//             IEnumerable<TExpected> expectation,
//             string because = "",
//             params object[] becauseArgs)
//             where TSubject : IEnumerable
//             where TAssertions : CollectionAssertions<TSubject, TAssertions>
//             => collectionAssertions.BeEquivalentByReferencesTo((IEnumerable)expectation, because, becauseArgs);
//     }
// }
