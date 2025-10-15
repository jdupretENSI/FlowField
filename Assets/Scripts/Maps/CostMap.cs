using UnityEngine;

public class CostMap
{
    public void Main(Cell[,] matrix)
    {
        //Use original tilemap to create a CostMap
        Debug.Log("CostMap.Main");
        
        //Create a tilemap with the info given
        
        
    }
}

//
// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using UnityEditor;
//
// /// <summary>
// /// Class to print text onto a Tilemap by string, position and destiantion Tilemap based on a bitmap font asset. How to: Add an empty gameObject to the scene and call it for instance MyTilemapTextManager and then add this script to it. Move the gameObject into the prefabs Folder of your projects Assets subfolder.
// /// </summary>
// public class MyTilemapTextManagerScript : MonoBehaviour
// {
//     public static MyTilemapTextManagerScript Instance; // to refer the singleton instance of the class
//
//     public Tilemap DestinationTilemap; // to provide a Tilemap surface to print on
//
//     [SerializeField]
//     private string _tilePalettesFolderNameWithinAssets; // to let the class search and load the Tiles unattended - there is no need to assign the Tiles in the inspector. - but it is possible
//     [SerializeField]
//     private string _tileNameStartsWithMask; // to filter the Tiles to load on unattended - The Tiles used in the TilePalette are filtered using this string - see https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
//
//     [System.Serializable]
//     private struct TileItem
//     {
//         public int Index;
//         public Tile Tile;
//     }
//
//     [SerializeField]
//     private List<TileItem> _tileItems; // filled by loading the tiles using _tilePalettesFolderNameWithinAssets and _tileNameStartsWithMask or manually setup using the inspector
//
//     private void Awake()
//     {
//         DontDestroyOnLoad(gameObject); // keep the instance of the parent empty game object over several scenes after it was once initalized/instantiated
//         if (Instance == null)
//         {
//             Instance = this; // make it resuable by referencing it statically using MyTilemapTextManagerScript.Instance.Print(...); Print is a public method defined in this class here!
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//         if (_tileItems == null || _tileItems.Count == 0)
//             fillTileItemCollectionByLoadingFromAssets(); // load the Tiles unattended and fill the _tileItems collection
//     }
//
//     private void fillTileItemCollectionByLoadingFromAssets()
//     {
//         if (_tileItems == null)
//             _tileItems = new List<TileItem>();
//         else
//             _tileItems.Clear();
//         var assets = AssetDatabase.FindAssets(_tileNameStartsWithMask, new string[] { _tilePalettesFolderNameWithinAssets }); // for instance "kromagrad_16x16_" and "Assets/TilePalettes"
//         int indexCount = 0;
//         foreach (var guid in assets)
//         {
//             Tile assetTile = AssetDatabase.LoadAssetAtPath<Tile>(AssetDatabase.GUIDToAssetPath(guid));
//             _tileItems.Add(new TileItem { Index = indexCount++, Tile = assetTile });
//         }
//         if (indexCount == 0)
//             Debug.Log($"Code 2012251527 - Unable to find any asset using the folder '{_tilePalettesFolderNameWithinAssets}' and the search string mask of '{_tileNameStartsWithMask}'!");
//         /* test it
//         foreach (TileItem tileItem in _tileItems)
//         {
//             print($"Tile Index='{tileItem.Index}', Tile Object='{tileItem.Tile}'");
//         }
//         */
//     }
//
//     private void Start()
//     {
//         Print("hello world", new Vector3Int(-5, -4, 0)); // ensure to assign a Tilemap in the inspector to DestinationTilemap before starting it
//     }
//
//     /// <summary>
//     /// Print a text on the Tilemap at a certain position.
//     /// </summary>
//     /// <param name="text"></param>
//     /// <param name="position">negative is left,bottom orientation; positiv is right,top orientation</param>
//     /// <param name="destinationTilemap"></param>
//     public void Print(string text, Vector3Int position, Tilemap destinationTilemap = null)
//     {
//         if (destinationTilemap != null)
//             DestinationTilemap = destinationTilemap;
//         if (text == null || DestinationTilemap == null)
//         {
//             Debug.Log("Code 2012251531 - Unable to print a text when the text or the destination Tilemap is null!");
//             return;
//         }
//         if (text != "")
//         {
//             text = text.ToUpper();
//             for (int i = 0; i < text.Length; i++)
//             {
//                 char ch = text[i];
//                 int chOffset = Convert.ToInt16(ch) - 33; // 'A' is ASCII code 65 ; 'A' is index 32 represented by asset 'kromagrad_16x16_32' ; the delta is therefore 65-32=33
//                 if (chOffset < 0)
//                     DestinationTilemap.SetTile(position, null);
//                 else
//                 {
//                     Tile tile = _tileItems[chOffset].Tile;
//                     if (tile == null)
//                         Debug.Log($"Code 2012251551 - Unable to index character '{ch}' by the use of an appropriate Tile!");
//                     else
//                         DestinationTilemap.SetTile(position, tile);
//                 }
//                 position.x++;
//             }
//         }
//     }
//
//
//
//
// }