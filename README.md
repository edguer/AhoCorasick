# .NET Core Aho-Corasick Implementation

This is a simple, memory friendly [Aho-Corasick for multiple string matching algorithm](https://en.wikipedia.org/wiki/Aho%E2%80%93Corasick_algorithm) in .NET Core. Aho-Corasick allows string matching in O(n), 'n' being the size of the string being searched. However, its automata requires a graph to be built in a Trie format, which might consume a lot of memory. This algorithm tries to reduce the amount of memory consumed by the Trie. Automata build time is not the priority here.

If you are interested on memory and time consumption, please run the "LoadTests" test project, logs are added accordingly.