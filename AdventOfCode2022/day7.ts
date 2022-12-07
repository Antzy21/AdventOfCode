import { dir } from 'console';
import * as fs from 'fs';

const input = fs.readFileSync("input7.txt", "utf-8");
const inputDemo = `$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k`

class File {
    constructor(n: string, s: number) {
        this.name = n;
        this.size = s;
    }
    name: string;
    size: number;
}

class Directory {
    constructor(name: string, parentDir: Directory | null) {
        this.name = name.trim();
        this.parentDir = parentDir;
    }
    name: string
    parentDir: Directory | null;
    subDirs: Directory[] = [];
    files: File[] = [];
    filesSize(): number {
        return this.files.reduce((totalSize, f) => {
            return totalSize += f.size
        }, 0);
    }
    dirSize(): number {
        return this.subDirs.reduce((totalSize, d) => {
            //console.log(d.dirSize())
            return totalSize += d.dirSize();
        }, this.filesSize());
    };
    sizeOfSubDirsWithSizeLessThan(n: number): number {
        return this.subDirs.reduce((acc, d) => {
            const dirSize = d.dirSize();
            if (dirSize < n) {
                acc += dirSize;
            }
            return acc + d.sizeOfSubDirsWithSizeLessThan(n);
        }, 0)
    }
    print (tabSize: number) {
        console.log("  ".repeat(tabSize)+"- "+this.name+" (dir)")
        this.subDirs.forEach(subdir => {
            subdir.print(tabSize+1);
        });
        this.files.forEach(file => {
            let line = "  ".repeat(tabSize+1)+"- "+file.name+` (file, size=${file.size})`;
            console.log(line);
        })
    }
}

function parseInput(input: string) : Directory {
    let baseDir = new Directory('/', null);
    const lines = input.split('\n');
    lines.reduce((curDir: Directory, line: string) => {
        if (line.includes("$ cd")) {
            if (line.includes("..")) {
                if (curDir.parentDir == undefined) {
                    throw "At top directory";
                }
                //console.log(`Going up to ${curDir.parentDir.name} dir`);
                return curDir.parentDir;
            }
            else if (line.includes("/")) {
                //console.log("Go to base dir");
                return baseDir;
            }
            else {
                let dirName = line.replace("$ cd ", "")
                let returnVal = curDir.subDirs.find((dir) => {
                    return dir.name == dirName;
                });
                if (returnVal == undefined) {
                    throw "Error finding sub dir";
                }
                //console.log(`Go to dir \"${returnVal.name}\"`);
                return returnVal;
            }
        }
        else if (line.includes("dir ")) {
            let dirName = line.replace("dir ", "")
            let newDir = new Directory(dirName, curDir)
            //console.log(`Create ${newDir.name} dir`);
            curDir.subDirs.push(newDir);
        }
        else if (line == "$ ls") {
        }
        else {
            const fileDetails = line.split(" ");
            let file = new File(fileDetails[1], parseInt(fileDetails[0]))
            //console.log(`Create ${file.name} file`);
            curDir.files.push(file);
        }
        return curDir;
    }, baseDir);
    return baseDir;
}

function getSmallestDirLargerThan(baseDir: Directory, n: number) : number {
    let smallestDirSize: number = Infinity;
    baseDir.subDirs.forEach(subDir => {
        let s = getSmallestDirLargerThan(subDir, n);
        if (s > n && s < smallestDirSize) {
            smallestDirSize = s;
        }
    });
    if (smallestDirSize != Infinity) {
        return smallestDirSize;
    }
    if (baseDir.dirSize() > n) {
        return baseDir.dirSize();
    }
    return NaN;
}

const baseDir = parseInput(input);
//baseDir.print(0);
const size = baseDir.sizeOfSubDirsWithSizeLessThan(100000);
console.log(size);

const n = - (70000000 - (baseDir.dirSize()) - 30000000)
//console.log("baseDir size:",baseDir.dirSize());
//console.log("n:",n);
const smallestDir = getSmallestDirLargerThan(baseDir, n)

console.log(smallestDir)