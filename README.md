# Haunted Woods
BUILD LINK: 

ABOUT: 

 
SCRIPTS:

        Enemies\

            EnemyAI: Manages enemy state (roaming or attacking) depending on player distance and fires the appropriate scripts
            
            EnemyHealth: Manages enemy health when attacked, fires flash and knockback scripts, and detects death of enemy
            
            EnemyPathfinding: Manages determining the enemy roaming position target and sets that as the new movement vector
            
            Grape: Manages spawning the grape projectile emitted by the Grape enemies and triggering its animation
            
            GrapeProjectile: Manages the animation curve of the projectile followed by the grape projectile and its shadow, and instantiates the grape projectile splatter upon impact with the ground
            
            GrapeSplatter: Manages the disabling of the grape splatter and ensures it has a residue damage field while active
            
            IEnemy: Interface for all enemy types containing Attack function
            
            Shooter: Manages the target cone of influence and burst timings for bullet projectiles spawned by the Ghost Shooter enemy type

            
        Inventory\

            ActiveInventory: Manages the smooth operation of the inventory by changing active weapons via either the num pad or the scroll wheel
            
            InventorySlot: Manages the proper weapon type into different inventory slots available

            
        Misc\

            Destructible: Manages the colliders of destructible game objects so that attacks can destroy them appropriately
            
            Dialogue_Emie: Manages the text box and audio spawned by characters when in proximity
            
            Flash: Manages the Flash animation whenever the Player or an enemy is hit, changing the material to a white flash for serialised amount of time
            
            Indestructible: Manages the game objects which are supposed to not be destroyed when hit
            
            InfoPost: Manages the text box spawned when Player walks in the trigger of signposts/bulletin boards
            
            Knockback: Manages the knockback force and effect to entities when attacked
            
            MouseFollow: Manages the rotation of weapons when the mouse is moved across the screen
            
            Parallax: Manages the slight parallax effect to the canopy and the twinkle particle system
            
            Pickup: Manages picking up the three different types of pickups (Health, Hunger, and Money) and their animation curves upon spawning
            
            PickupSpawner: Manages the randomised instantiation of the different pickup types
            
            RandomIdleAnimation: Manages the random animations of flowers, torches, water etc to ensure asynchronous animation among different instances of same game object
            
            SpriteFade: Manages the transparency added to specific layers (canopy, trees) when the Player walks underneath them
            
            Transparency: Manages the transparency added to entire scene upon scene transitions

            
        Player\

            ActiveWeapon: Manages the cooldown and registering of attack button of the active weapon 
            
            DamageSource: Manages the colliders of weapons to damage enemies
            
            PlayerHealth: Manages the Player health upon healing, taking damage and death
            
            PlayerController: Manages Player input, movement, and dashing routine
            
            SelfDestroy: Manages self-destruction of particles systems etc
            
            Hunger: Manages the hunger levels of the player by refilling over time and preventing dash while empty

            
        SceneManagement\

            AreaEntrance: Manages the proper Player and Camera instantiation upon scene transition
            
            AreaExit: Manages the proper scene transition upon stepping into the portal trigger collider
            
            BaseSingleton: The Base Singleton class responsible for defining all game singletons
            
            CameraController: Manages the Cinemachine Camera to properly follow the Player upon scene transitions
            
            EconomyManager: Manages the Money shown in the UI 
            
            MainMenuUI: Manages the Main Menu scene and enables different transitions
            
            SceneManagement: Manages the proper transition of scenes by being a DontDestroyOnLoad singleton
            
            ScenePartLoader: Manages the additive subscene loading in the open world scene
            
            ScreenShakeManager: Manages the slight screen shake effect on Cinemachine upon taking damage
            
            Singleton: Manages the definition of Singletons for the game
            
            UIFade: Manages the UI fading during scene transitions

            
        Weapons\

            Bow: Manages the attack function for the Bow weapon by instantiating an arrow projectile
            
            IWeapon: Interface for all weapon types containing attack and weapon info functions
            
            MagicLaser: Manages the instantiation of the laser when using attack of the Staff weapon
            
            Projectile: Manages the logic for projectiles (range, move speed etc) and enables it to damage enemies
            
            Staff: Manages the attack function for the Staff weapon and fires the MagicLaser script
            
            Sword: Manages the attack function for the Sword weapon and activates/deactivates the sword collider accordingly
            
            WeaponInfo: Scriptable Object containing the weapon information such as the prefab, cooldown, damage,  and range
