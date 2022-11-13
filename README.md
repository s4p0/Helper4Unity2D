# Helper4Unity2D
A set of classes to help with boring task

# Final Result

Create Sprite Library Assets easier

![Sprite Libraries](images/06_libs_created.png)

![Sprite Libraries](images/07_library.png)



# How it Works

## Prepare your sliced sprites
First, slice your spritesheet as you usually do, multiple, open Sprite Editor and cut them by its dimensions.

The image below, shows an image with 7 actions. One row each.

![Spritesheet #01](images/01_single_texture.png)

Also, I have the very same configuration for another image, different 'skin'

![Spritesheet #02](images/02_single_texture.png)

After sliced, looks like that

![All sprites within an image](images/03_sliced_texture.png)

## Create a new GameObject in the scene
For this, I renamed according to the many 'characters' I have to import

![GameObject](images/04_scene_setup.png)

## Final step (to use, righ click the script in GameObject)
Add the new script (SpriteLibraryCreator) to that very same GameObject. 
- Name the library you are creating...
- Add many animations as it is required (my case, 7)
- Name all the animations
- Provide the sprite count it is required for that single animation

![Final Result](images/05_spritelibrarycreator.png)

- Right click, script to execute the creator
