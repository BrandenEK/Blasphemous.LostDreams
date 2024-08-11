# Adding NPCs

## Step 1 (Add npc info):
1. Review this file to see the data format of the npc [NpcInfo.cs](../../Blasphemous.LostDreams/Npc/NpcInfo.cs)
2. Navigate to the resources/data/Lost Dreams/npcs.json file
3. Add an npc object with the following properties to the json file:

| Name | Description | Required |
| ---- | ----------- | :------: |
| id | The id of the npc (Must be unique) | yes |
| animation | The id of this npc's idle animation (Must match something in the animations.json file) | yes |
| colliderWidth | Width of this npc's interactable hitbox | yes |
| colliderHeight  | Height of this npc's interactable hitbox | yes |

4. Copy the json into https://jsonlint.com to validate it, then copy it back into the json file to format it

## Step 2 (Add level edits):
1. Navigate to the resources/levels/Lost Dreams folder
2. Add a new file called {SceneId}.json
3. Add a level addition object with the following properties to that json file:

| Name | Description | Required |
| ---- | ----------- | :------: |
| type | Must be ```npc``` | yes |
| id | The id of the npc that should appear here | yes |
| position | XYZ coordinates of the npc | yes |
| condition  | In-game condition for this npc to appear | no |
| properties | List of dialog ids that this NPC can say (Can be used with conditionals to select different ones, the first one that evaluates to true is used) | yes |

---

Example npcs.json:
```json
[
    {
        "id": "NPC01",
        "animation": "NPC01_idle",
        "colliderWidth": 1.2,
        "colliderHeight": 2
    },
    {
        "id": "NPC02",
        "animation": "NPC02_idle",
        "colliderWidth": 3,
        "colliderHeight": 4
    }
]
```

Example D01Z02S01.json:
```json
{
    "additions": [
        {
            "type": "npc",
            "id": "NPC01",
            "position": {
                "x": 100,
                "y": 5
            },
            "condition": "flag:DEFEATED_BS01",
            "properties": [
                "MET_NPC01:NPC01_normal",
                "NPC01_meet"
            ]
        }
    ]
}
```
