# My humble beginnings (personal archived project)
*Learning to code is a unique and fun experince. Even though there is still a lot to learn, it's just not the same as it used to be.
Being self-taught and starting out is something I still look back on. Unfortunately, there was never much to remember
it by, since I left no traces of my learning journey (besides few broken executables). That is until recently, when I discovered one of my old projects
from May 2016 on an old laptop. It was written in Microsoft Visual C# 2010 Express on the good old .NET Framework 4.* 

## The project
The project is called **ConSole** (for reasons unknown). It allows you to create and run simple programs from a console window.
When editing code, you can use these interactive commands:
- help - show available commands
- save - save code changes
- exit - exit the code editor
- back - goes back to the last line
- redo [number] - goes back to the specified line
- insert [number] - insert new line after the specified line and goes to it

These are built-in functions:
- print - prints an empty line
- print [variable] - prints value of the variable on a new line
- end - all programs must be terminated with end, works as `return`
- scan - reads a line of input
- scan [variable] - read a line of input and save it to the variable
- number [variable] - declare new variable of type `number` (32-bit)
- text [variable] - declare new variable of type `text` (UTF-16 string)
- math [variable] [op]... - perform operations on a variable; operations are formatted as `+1`, `-1`, `*1` or `/1` (variables are fine too)
- set [variable] [value] - set value of a variable
- if [condition] (...) - parentheses must be on separate lines; examples of conditions: `a = b`, `a ! b`, `a > b`, `a < b`

The whole thing is a little broken and does a lot of file manipulation (I had to run it as administrator to make it work).
Few observations from the code:

```cs
int AntiBug;
int Bug = 0;
if (wNumber > Radek - 1)
    AntiBug = 1 / Bug;
```
*Apparently this is how I solved bugs.*

```cs
string[] radky = new string[500];
// ...
if (radku > radky.Length)
{
    Console.WriteLine("Soubor je příliš dlouhý!"); // File is too long
    Continue = false;
}
```
*I didn't know what Lists are?...*

```cs
Console.WriteLine("Debugging...");
List<string> Bugs = new List<string>();
```
*...except I did.*

```cs
using (StreamWriter sw = new StreamWriter("ram.txt")) { }
```
*Saving runtime variables into a text file. I was a real prodigy.*

```cs
else if (r.StartsWith("while "))
    Ok = true;
```
*While loop is named as one of the commands, but isn't actually implemented.*

```cs
ConsoleKeyInfo Key = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false);
bool Next = false;
while (Next == false)
{
    if (Key.KeyChar.ToString().ToUpper() == "N" || Key.KeyChar.ToString().ToUpper() == "O" || Key.KeyChar.ToString().ToUpper() == "S" || Key.KeyChar.ToString().ToUpper() == "K")
        Next = true;
    else
    {
        Console.Clear();
        Console.WriteLine("Vytvořit nový projekt [n]");
        Console.WriteLine("Otevřít projekt [o]");
        Console.WriteLine("Spustit projekt [s]");
        Console.WriteLine("Ukončit konzoli [k]");
    }
    Key = Console.ReadKey();
}
```
*This causes a bug where you always need to press the key twice. After 5 years I still remember not being able to fix this. (The fix is moving ReadKey to start of the loop)*

## Caution:
Reading the code may cause severe headaches.
