# Adding animations

### Step 1 (Design animation):
1. Design the animation
2. Save it as a png file, layed out as a spritesheet in one row

### Step 2 (Upload animation):
1. Name it something unique and descriptive, like ```NPC01_idle.png``` or ```perpetua_attack.png```
2. Upload the file to a subfolder in the data folder (No png files should be in the same folder as animations.json, put it in ```npcs/``` or ```enemies/``` etc)

### Step 3 (Add animation info):
1. Review this file to see the data format of the animation [AnimationImportInfo.cs](../../Blasphemous.LostDreams/Animation/AnimationImportInfo.cs)
2. Navigate to the resources/data/Lost Dreams/animations.json file
3. Add an animation object with the following properties to the json file:

| Name | Description | Required |
| ---- | ----------- | :------: |
| name | The name of the animation (Must be unique) | yes |
| filePath | The path to the image file, starting from this folder | yes |
| width | Number of pixels wide each frame is | yes |
| height | Number of pixels tall each frame is | yes |
| secondsPerFrame | The time to spend on each frame of the animation | yes |

4. Copy the json into https://jsonlint.com to validate it, then copy it back into the json file to format it

---

### Example animations.json:
```
[
    {
        "name": "ANIM_NAME",
        "filePath": "npc/test.png",
        "width": 32,
        "height": 32,
        "secondsPerFrame": 0.1
    },
    {
        "name": "ANIM_NAME_2",
        "filePath": "npc/other.png",
        "width": 32,
        "height": 32,
        "secondsPerFrame": 0.5
    }
]
```
