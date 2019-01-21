# teaching-c

![teaching-c](https://raw.githubusercontent.com/yassineaddi/teaching-c/master/teaching-c.png)

It is a quick and dirty graphical user interface application for executing a very small subset of C language, with memory visualization. It was created in my process of practicing and understanding interpreters and compilers, the UI is inspired by @fredoverflow's [skorbut](https://github.com/fredoverflow/skorbut-release), unlike skorbut, it is open-source. Oh and it is worth mentioning the informative [series](https://ruslanspivak.com/lsbasi-part1/) of Ruslan Spivak.

This wasn't an attempt to actually implement or support the whole C-syntax, I was just playing around and only a few naive concepts are included due to lack of time and to keep it stupid, that's why I'm probably not going to add further features; to mention what it can currently doing:

+ `int`, `float`, `double` and `char` data types.
+ Declaring and defining `subroutines`, passing `parameters` and `return` results.
+ Increment `++` and decrement `--` operators (suffix and prefix).
+ Unary & binary operators: `+`, `-`, `*`, `/`, `%`, `==`, `!=`, `>`, `>=`, `<`, `<=`, `&&`, `||` and `( )` for expressions.
+ Compound & null stataments.
+ Escape characters `\n`, `\t` and `\0`.
+ Integer, real (floating-point) and octal & hexadecimal literals.
+ `do while`, `while` and `for` loops.
+ Conditional statments `if else`.

License
-------

```
Copyright 2018 Yassine Addi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```
