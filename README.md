# Coordinated Movement System For AI


Coordinated movement systems are a powerful tool for achieving a variety of tasks and goals. By enabling multiple autonomous agents or robots to move in a coordinated manner, these systems can be used to achieve objectives that would be difficult or impossible to accomplish using individual robots. Basically, it's a group that moves at the same speed, takes the same path, and arrives at the same time. 
 


## Goal and Result

The goal of this project is to create a coordinated movement system using AI. The system will be designed to control a group of agents, in this case we will be calling them soldiers, in a coordinated manner with the aim of achieving a common task or goal. The system will be based on the AI class and will be able to move the robots accordingly depending on the given situation, this way the system can adapt and improve over time. The end goal of the project is to have a robust and efficient coordinated movement system that can be applied to a RTS type game.
 
 
 
![](https://github.com/joquillan/CoordinatedMovementSystem/blob/main/Images/Circle%20Formation.gif)
 


### Current Different Classes

The classes currently have no real difference except for color.

- King: The king will always be the center of the formations. It will always be surrounded or leading the soldiers. There can only be one king at the moment. 
- Rangers: Their position will always be at the back or middle.
- Sword: The typical knight, these will position between the rangers ans shields.
- Shields: Meant to be defense type soldiers, these will always be at the front or the furthest away from the center.

## RTS Selection, controls and Navigation


### RTS Selection
We are using a typical and basic Selection System. You can select soldier by draging, clicking and/or shift-ckicking. 


//gif of showing it


###Controls

* "WASD" to move the camera
* Scroll to zoom in and out.

* "1 key": Normal Navigation, No Formation.
* "2 key": Circle Formation.

* "Q": Spawn One Soldier.
* "E": Spawn Five Soldiers.

* "5 key": Make The Selected soldier The King class.
* "6 key": Make The Selected soldiers The Sword class.
* "7 key": Make The Selected soldiers The Shield class.
* "8 key": Make The Selected soldiers The Ranger class.
 

### Navigation

We are using unity's bild in navigation system. Unity's NavMeshAgent is a component that allows for autonomous movement of characters in a game or simulation using pathfinding and navigation. It provides an easy-to-use and efficient way to implement autonomous navigation, and can be integrated with other components/systems.




//gif? of this?




# Implementation

Now that we have different classes and we have to base our soldiers psition according to the class, we need to know how many different classes we have selected and how many soldiers of each class. However, there are also challenges to using polar coordinates for circle formation. One of the main challenges is that it can be difficult to keep track of the robots' positions and movements, especially if the formation is large or complex. Luckily, the robots are already equipped with a navigation system to make this a little easier.


### The Leader

Selecting the leader of a coordinated group system can have a significant impact on the overall performance and effectiveness of the group. There are several different approaches that can be used to select a leader, including randomly selecting a soldier, using a virtual leader, or selecting the most important soldier, the king.


* Randomly selecting a soldier as the leader can lead to a more decentralized system where each unit has an equal chance of leading the group.


* Using a virtual leader can be useful in situations where there is not a king present. Virtual leaders can be programmed with specific skills or abilities that make them well suited to lead the group. This virtual leader can be placed anywhere in the formation without the player knowing where and it gives us more control over the formation.


* Selecting the most important soldier,the king as the leader can be a more effective approach. This type is used more often in videogames, where one soldier will be chosen by the player. Using this has the advantage of making the formation protect the leader and base the formation solely around that leader. However, this approach can also lead to an overreliance on that single soldier, which can be problematic if the soldier becomes unavailable or doesn't exists.


I decided on using the both the virtual one and most important one. In the circle formation, we will only be using the virtual target, since there won't always be a king in the selected soldiers. The king will always take the virtual leaders place, but in reality the formation is based on the virtual target. In the rectangular formation( which isn't finished) I planned on using the king if it was avaible, else I would make a virtual leader and make it take his place.


## Circle Formation



// another foto of formation




### Polar Coordinates

One example of this system, is the formation of a circle using polar coordinates. Polar coordinates are a way of specifying the position of a point in two-dimensional space. Rather than using Cartesian coordinates (x and y), polar coordinates specify the position of a point based on its distance from a central point (radius) and the angle between that point and a reference direction (θ). This makes polar coordinates particularly well suited for circle formation, as they allow for easy adjustment of the circle's size and shape.

To form a circle using polar coordinates, each robot is given a set of coordinates (radius and angle) that specify its distance and angle from the central point. These coordinates are then used to calculate the robot's position in Cartesian coordinates, which can be used to control its movement.

One of the main advantages of using polar coordinates for circle formation is that it allows for easy adjustment of the circle's size and shape. By changing the distance and angle of the robots' coordinates, the circle can be made larger or smaller, (or even transformed into an ellipse, but not yet in this project). Additionally, because all robots are using the same coordinates system, it is easy for them to share their positions and movements with each other, helping to ensure that the formation remains coordinated and stable.

### How The Circle Gets Formed


```
First we check how many classes in total of the selected soldiers and how many soldiers per class

we add this to a KeyValuePair list. We add the class closest to the center first.

List<KeyValuePair<Soldier.SoldierClass, int>> soldierClassInfo = new List<KeyValuePair<Soldier.SoldierClass, int>>();

The total amount of classes is a start of total amount of circles.
int totalCircles = The total of different classes

spacing = 3;

for ( int i = 0; i < totalCircles; i++)
{
    int addedCircles = CalculateCircle(radius, soldierClassInfo[i]);
    if (addedCircles != 0)
    {
        radius += spacing * addedCircles;
    }
    radius += spacing;

}

```


Right now we have a circle per class and the order of the classes is already good. The Calculate Circle Function's job is to calculate all that classes soldiers to the right position using polar coordinates. 

We still have one problem, if we have many soldiers, they won't fit on a single circle. So the Calculate Circle Function will take that into account and will return an integer of the amount of added circles. 

This function takes the current radius, a class and the amount of soldiers for that class



```
 
 int remainingSoldiers = currentSoldiers.Count;
 soldierIndex = 0
 do
 {
     maxAmountSoldiersOnCircle = The circles periphery / soldierwidth * spacingBetweenSoldiers

     //if the amount of soldiers is bigger than it can fit on circle -> create new circle
     
     if (maxAmountSoldiersOnCircle > remainingSoldiers)
     {
         for  the remaining soldiers
         {
            currentSoldier[soldierIndex + i] = calculate the coordinates and set destination
         }
         remainingSoldiers = 0;
     }
     else
     {
         for the max amount of soldiers on that circle
         {
             currentSoldier[soldierIndex + i] = calculate the coordinates and set destination
             ++soldierIndex;
         }
         
         ++addedCircles;
         radius += spacing;

         remainingSoldiers -= maxAmountSoldiersOnCircle;
     }
 }
 while remaining soldiers isnt 0

 return addedCircles;

```






In this GitHub repository, we provide an open-source implementation of a coordinated movement system for circle formation using polar coordinates. Our implementation includes a set of functions and classes for calculating and controlling the robots' positions and movements, as well as a sample application that demonstrates how to use the system. We hope that this implementation will be useful for anyone looking to develop their own coordinated movement systems, or for anyone interested in learning more about the use of polar coordinates for circle formation.



## Rectangular Formation







