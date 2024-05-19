# Block Blast

Welcome to Block Blast! Block Blast is a fun, casual, match-2 puzzle game designed for mobile devices.

---

## Table of Contents

1. [Introduction](#introduction)
2. [Gameplay](#gameplay)
3. [Development and Implementation](#development-and-implementation)
4. [Demo Video]([https://drive.google.com/file/d/19SKU6MNbh5U8-fR3Ld8rImCz67wYGqBy/view](https://drive.google.com/file/d/1yX0myTz7z-EOYZnpIXJnje3M-4l2IJnX/view?usp=sharing))

---

## Introduction

Block Blast is a mobile match-2 puzzle game aimed at casual gamers and puzzle enthusiasts of all ages. The goal is to match two or more blocks of the same color to clear obstacles.

---

## Gameplay

### Core Mechanics
- **Tap to Match**: Tap on two or more adjacent blocks of the same color to clear them from the board.
- **Scoring**: Earn points for every block cleared. Higher scores for larger matches.

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
