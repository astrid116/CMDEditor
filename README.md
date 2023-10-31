# CMDEditor

# Usage

Enter file path (hit enter to use current directory)

Enter the file name that you want to edit

It should look like this:


![Screenshot_1773](https://github.com/astrid116/CMDEditor/assets/147720887/7e00496e-5361-4889-9a89-f41040643106)


Hit enter

To insert a line, specify the line where you want to insert a value. You can do this by specifying a positive integer, then writing the value of the line (For example, if you want to insert "Hello, World!" to line 10, use "10 Hello, World!")


![Screenshot_1775](https://github.com/astrid116/CMDEditor/assets/147720887/aebb935f-5472-44e5-8d4d-9c6e2f2259c5)


In case if you want to insert a line under a specified line, write "+", then the specified line as a positive integer and the line you want to insert. (For example, if you want to insert "Hello, World" under line 14, use "+ 14 Hello, World!")


![Screenshot_1776](https://github.com/astrid116/CMDEditor/assets/147720887/e9648a6d-df74-4163-a71d-5837817d289f)


If you want to list the file that you're currently editing, hit enter without specifying anything.


![Screenshot_1777](https://github.com/astrid116/CMDEditor/assets/147720887/2004354b-e025-4de2-b07d-db1832260e22)


# Commands

:q - Exits from the editor.

:clear - Clears the console.

:clearfile - Clears the file.

:removeemptylines - Removes all the empty lines from the file.

:lastmodified - Shows you when was the last time you edited the file **with this program (ONLY LOGS IF YOU EXIT FROM THE FILE USING :q)**
