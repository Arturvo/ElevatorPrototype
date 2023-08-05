# ElevatorPrototype - Documentation
### ElevatorPrototype was created as a recruitment task to implement elevator functionality in Unity 3D, with the following requirements:
1. The elevator needs to have a door opened with a button. The door contains a photocell that prevents the player to be squished. The door opens using Unity animator.
2. Inside the elevator, there is a panel with buttons leading to different floors. After pressing a button, the door closes and starts traveling to the selected floor. After reaching the destination, the door opens.
3. Elevator plays different sounds when starting moving and finishing movement.
4. Every floor has a button allowing the door to be opened.
5. The project should not use singleton pattern, nor methods such as GameObject.Find and Object.FindObjectOfType.

### Third party plugins used:
Dependency injection - Extenject: https://assetstore.unity.com/packages/tools/utilities/extenject-dependency-injection-ioc-157735  
Outlines - Quick Outline: https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488  
Audio Manager - Audio Manager | CG: https://assetstore.unity.com/packages/tools/audio/audio-manager-cg-149123  
Scene autoload - DeusaldUnityTools: https://github.com/Deusald/DeusaldUnityTools  
Other third party content: all sound clips

### Assembly structure
The project uses 2 main assemblies to organize the code:  
Core - contains more abstract systems that can be used in different projects  
Game - contains implementation specific to the project  

Additionally, each plugin uses its own assembly.

Assembly structure is simple:  
Game -> Core  

### Scene setup
Project is separated in multiple scenes:  
Init - a scene that is always loaded at the start, regardless of which scene was opened in the editor. Used to make sure all global managers are fully initialized before being accessed  
UserInterface - contains user interface  
Player - contains the player so that it can exist separately to the floors  
Elevator - contains the elevator so that it can be shared between floors  
FloorX - each floor is a separate scene. When  the player changes floors, the previous floor scene is unloaded and the new floor is loaded in its place 

### Dependency injection
The project is initiated by Extenject creating a ProjectContext persistent object with all global managers as children (ProjectContext prefab is located in Resource folder). After that, SceneLoader loads necessary scenes. ProjectContext uses a mono installer to bind global managers' interfaces to specific scripts. Those can later be accessed by injecting them from anywhere.
