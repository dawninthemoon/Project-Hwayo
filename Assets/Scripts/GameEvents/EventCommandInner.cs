using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTilemap;

public partial class EventCommand {
    public class SharedData {
        public CustomGrid<TileObject> Grid { get; }
        public Dictionary<string, LevelInfo> LevelDictionary { get; }
        public PlayerControl Player { get; }
        public EntityInfo ExecuterEntity { get; set; }
        public EnemyCreation EnemyCreation;
        public SharedData(PlayerControl player, CustomGrid<TileObject> grid, Dictionary<string, LevelInfo> info, EnemyCreation enemyCreation) {
            Player = player;
            Grid = grid;
            LevelDictionary = info;
            EnemyCreation = enemyCreation;
        }
    }
    public class SharedVariable {

    }
}
