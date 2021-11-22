# Learn Sequential Logic in Time Dependant Processes

[![  Run Tests on Godot 3.3  ](https://github.com/Kersoph/open-sequential-logic-simulation/actions/workflows/run_wat_tests.yml/badge.svg)](https://github.com/Kersoph/open-sequential-logic-simulation/actions/workflows/run_wat_tests.yml)

Build solutions for real and fictional scenarios where cyclic working controllers work in time dependant environments (e.g. Programmable Logic Controller PLC).
A programming language close to Sequential Function Chart (SFC EN 61131-3 / IEC 61131-3) is used while it is intended to add more languages later. SFC is used to learn the basic principles as it focuses exactly on the core elements of sequential logic and the control of serial or parallel tasks, is easy to learn, has similarities to petri-nets, GRAFCET, Activity Diagrams, State Machines or Statecharts and can be used to program PLCs.



### Select a Lesson

The lessons focus on different topics and have an increasing difficulty within the topics.

![LandingPage](https://user-images.githubusercontent.com/26461040/142898955-ec21c2c4-4bb7-4f1c-93f2-407f663d05b4.png)

### Create your solution and test it

You can add breakpoints, pause the execution, reset the PLC, reset the simulation, inspect your I/O table, and observe the plant.

![Edit](https://user-images.githubusercontent.com/26461040/137284868-7041e6fb-3c62-4564-b4c5-8e14fd8f6e27.png)

### Get automated feedback

Iterate over your solutions and see what will happen with your approaches.

![Test](https://user-images.githubusercontent.com/26461040/137284892-abd2356a-6b08-4fc4-8336-2e4acf7774a4.png)



## Download

Select the latest version under [Releases](https://github.com/Kersoph/open-sequential-logic-simulation/releases) on the right side.



## How to build it yourself

1. Download [Godot Mono](https://godotengine.org/download) version
2. Follow the mono version [installation](https://docs.godotengine.org/en/stable/getting_started/scripting/c_sharp/c_sharp_basics.html)
    - If you just want to compile it, you only need to follow the first step: Install ".NET Core SDK" e.g. from  https://dotnet.microsoft.com/download/dotnet
    - There are also many tips on that page on how to set up external editors and debuggers.
3. Download this project (https://github.com/Kersoph/open-sequential-logic-simulation) (Clone or download as zip)
4. Open Godot Mono and open the downloaded project with it
5. Press F5 or the small play button on the top right corner


## How to contribute

### Reporting bugs

**Always open one issue for one bug.** If you notice several bugs and want to report them, make sure to create one new issue for each of them.

Please provide **detailed information on how to reproduce the bug** and attach save files or screenshots if it could help. Include your platform you are working on and which version you are using. Making your bug report easy to reproduce will make it easier for contributors to fix the bug.

### Contributing pull requests

Please make sure the functionality is desired. Keep in mind that this is a **simulation tool to learn the basic principles of sequential logic in time dependant processes**. Open a feature proposal if you are unsure if your feature is needed or how to implement it best. For bugfixes, please refer to the issue in the issue tracker.

Please follow the [C# style guide](https://github.com/Kersoph/open-sequential-logic-simulation/blob/main/c-sharp-style-guide.rst) and use **clean code**.

**Write Unit Tests**. Have a look on other tests in the folder [wat_tests](https://github.com/Kersoph/open-sequential-logic-simulation/tree/main/wat_tests).

Format your commit messages with readability in mind: A Git commit message is formatted as a **short title (first line)** and an **extended description (everything after the first line)**.

## Background

Learning the fundamental principle of sequential logic in time dependent processes is critical for system engineers. The aim of this project is to investigate how using modern interactive technologies informed by pedagogical principles in learning applications, specifically, those with gamifications can help the students compared to the current theoretical approaches based on text or passive media.
