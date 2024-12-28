<h1 align=center>GHOST in the POWERSHELL</h1>

<!-- TODO: Add and embed overview video or GIF here-->
```

                                                                                
                                                                       ██████  █
                                                                █████████████  █
                                                           ██████████████████  █
                                                     ████  ██████████████████  █
                                                 ████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                             ████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                             ████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ............................████████  ██████████████████  █
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ............................████████  ██████████████████  █
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx████████  ██████████████████  █
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx████  ██████████████████  █
###########################################################██████████████████  █
################################################################█████████████  █
```

# Table of Contents
- [About](#about)
- [Installation](#installation)
- [Credits](#credits)

## About
**GHOST in the POWERSHELL** is a 3D Maze Command-Line Game made possible with the ray casting tech inspired from classic games like *Wolfenstein* and *DOOM*.

find the statue in each 3 levels to finish the game. Do it before the timer however, because if your time is out, its game over! 👻

## Installation

<details>
<summary><b>Windows</b></summary>

before all else, make sure you have dotnet ≥ v8.0 installed for your system already.
[dotnet 8.0 link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## From source
  - Clone the repository.
  - open your terminal, and navigate through the repo's directory
```sh
# cd (change directory)
$ cd /path/to/project/ghost-in-the-powershell

# cd to src/
$ cd src/
```
  - then in the `src/` directory, run:
  ```sh
  # restore nuGet package dependencies
  $ dotnet restore
  
  # run the program
  $ dotnet run
  ```

</details>

<details>
<summary><b>Linux</b></summary>

before all else, make sure you have dotnet ≥ v8.0 installed for your system already:

```sh
# Arch Linux
$ sudo pacman -S dotnet-sdk

# Debian
$ sudo apt install dotnet-sdk-8.0

# Fedora Linux
$ sudo dnf install dotnet-sdk-8.0
```
for any other distros, look on how to install dotnet for your respective package manager. you can always consult the official website: [dotnet 8.0 link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

(also, im not sure if dotnet 9.0 works, but it should i believe)

### From Source
  - Clone the repository.
  - open your terminal, and navigate through the repo's directory
```sh
# cd (change directory)
$ cd /path/to/project/ghost-in-the-powershell

# cd to src/
$ cd src/
```

  - then in the `src/` directory, run:
  ```sh
  # restore nuGet package dependencies
  $ dotnet restore
  
  # run the program
  $ dotnet run
  ```
</details>

## Contributing (?)
its not like I have any plans to maintain this project, but if for any reason you want to contribute anything, feel free to clone the repository and make a pull request.

## Credits 🎉
- [javidx9](https://www.youtube.com/@javidx9) for his youtube video tutorial on the ray caster inspired from *Wolfenstein* in the commandline, originally written in C++
    -  [video link](https://www.youtube.com/watch?v=xW8skO7MFYw)
- [NetCoreAudio](https://github.com/mobiletechtracker/NetCoreAudio) for the cross-platform audio library
