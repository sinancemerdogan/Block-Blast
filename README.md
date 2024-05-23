# Block Blast (Game Architecture With Scriptable Objects)

Welcome to Block Blast! Block Blast is a fun, casual, match-2 puzzle game designed for mobile devices.

---

## Table of Contents

1. [Introduction](#introduction)
2. [Gameplay](#gameplay)
3. [Development and Implementation](#development-and-implementation)
4. [Custom Editors](#custom-editors)
5. [Demo Video](https://drive.google.com/file/d/1yX0myTz7z-EOYZnpIXJnje3M-4l2IJnX/view?usp=sharing)
   
---

## Introduction

Block Blast is a mobile match-2 puzzle game aimed at casual gamers and puzzle enthusiasts of all ages. The goal is to match two or more blocks of the same color to clear obstacles.

---

## Gameplay

### Core Mechanics
- **Tap to Match**: Tap on two or more adjacent blocks of the same color to clear them from the board.
- 
---

## Custom Editors

### Level Editor

![level-editor](https://github.com/sinancemerdogan/Block-Blast/assets/72517285/e4a7230f-ce31-4308-b2ab-326da3af2779)

### Block Editor

![block-editor](https://github.com/sinancemerdogan/Block-Blast/assets/72517285/ce8fab13-e51b-43b0-aa18-30c93a04dd87)

---

## Development and Implementation

### Object-Oriented Programming

#### Inheritance
Every block in the game inherits from the `Block` abstract class. This allows us to treat every different kind of block as a generic block, promoting code reusability and simplicity.

![Block Inheritance](https://github.com/sinancemerdogan/Block-Blast/assets/72517285/59208858-b53e-42eb-952b-c6c270c0e0ce)

#### Composition
Composition is used to build complex behaviors by combining simple, reusable components. To achieve this, we use interfaces to define specific behaviors. 

##### Interfaces
Each block implements the interfaces corresponding to the behaviors it needs to exhibit. This approach allows for greater flexibility and modularity, as blocks can easily adopt new behaviors by implementing additional interfaces.

![IFallable Interface](https://github.com/sinancemerdogan/Block-Blast/assets/72517285/8c23dc9c-7a19-482c-9087-7d9336561229)


![Cube Block](https://github.com/sinancemerdogan/Block-Blast/assets/72517285/57528557-5246-4e63-a569-f8daf97dc75a)


### Design Patterns
#### Factory
#### Observer
#### Delegate

### Optimization Techniques
#### Object Pooling
#### Static Dynamic Canvas Separation
#### Event Driven Actions

### Scriptable Objects
#### Scriptable Object Events
#### Scriptable Object Event Channels
#### Scriptable Object Variables
#### Scriptable Object Components or Systems

### Tools and Technologies
- **Game Engine**: Unity
- **Programming Language**: C#

---

## Disclaimer

This project is an educational exercise and is not intended for commercial use. It contains game assets from Toon Blast (developed by Peak Games) and Royal Match (developed by Dream Games). All rights to these assets remain with their respective owners. The use of these assets is solely for the purpose of learning and personal development. No part of this project may be used for commercial purposes or distributed without appropriate permissions.
