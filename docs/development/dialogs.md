# Adding dialogs

## Step 1 (Add dialog info)
1. Review this file to see the data format of the dialog [DialogInfo.cs](../../Blasphemous.LostDreams/Dialog/DialogInfo.cs.cs)
2. Navigate to the resources/data/Lost Dreams/dialogs.json file
3. Add a dialog object with the following properties to the json file:

| Name | Description | Required |
| ---- | ----------- | :------: |
| id | The name of the dialog (Must be unique) | yes |
| type | "Lines", "GiveObject", "GivePurge", "BuyObject", "PurgeGeneric" | yes |
| textLines | List of localization keys in the dialog text | yes |
| responseLines | List of localization keys in the dialog response | no |
| item | Id of the item to give or take | no |

4. Copy the json into https://jsonlint.com to validate it, then copy it back into the json file to format it

## Step 2 (Add localization)
1. Navigate to the resources/localization/Lost Dreams.txt file
2. Add the key-value pair for each text/response line for each language
   - Key: The id of this dialog line, should match something in lines list in the dialog object
   - Value: The actual text to be displayed

#### Note: All of this will probably be changed very soon

---

Example dialogs.json:
```json
[
    {
        "id": "NPC01_meet",
        "type": "Lines",
        "textLines": [
            "NPC01_meet_1",
            "NPC01_meet_2"
        ]
    },
    {
        "id": "NPC01_item",
        "type": "GiveObject",
        "textLines": [
            "NPC01_item_1"
        ],
        "item": "PR501"
    }
]
```
