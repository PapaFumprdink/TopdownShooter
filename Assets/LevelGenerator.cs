using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class LevelGenerator : MonoBehaviour
{
    [SerializeField] private string m_Seed;
    [SerializeField] private Tilemap m_WallMap;
    [SerializeField] private Tile m_WallTile;

    [Space]
    [SerializeField] private Vector2Int m_MinRoomSize;
    [SerializeField] private Vector2Int m_MaxRoomSize;
    [SerializeField] private AnimationCurve m_RoomSizeDistribution;

    [Space]
    [SerializeField] private int m_RoomCount;
    [SerializeField] private int m_RoomBorder;
    [SerializeField] private int m_RoomSpacing;

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    private void GenerateEditor()
    {
        m_WallMap.ClearAllTiles();
        Generate();
    }

    private void Generate()
    {
        System.Random random = new System.Random(m_Seed.GetHashCode());

        Bounds[] generatedRooms = new Bounds[m_RoomCount];

        for (int i = 0; i < m_RoomCount; i++)
        {
            Vector3Int rootPosition = Vector3Int.zero;
            Vector3Int size = new Vector3Int
            {
                x = (int)Mathf.Lerp(m_MinRoomSize.x, m_MaxRoomSize.x, m_RoomSizeDistribution.Evaluate((float)random.NextDouble())),
                y = (int)Mathf.Lerp(m_MinRoomSize.y, m_MaxRoomSize.y, m_RoomSizeDistribution.Evaluate((float)random.NextDouble())),
                z = 1,
            } + new Vector3Int(m_RoomSpacing * 2, m_RoomSpacing * 2, 0);

            float angle = (float)random.NextDouble() * Mathf.PI * 2f;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

            int tries = 0;
            bool doesFit = true;
            Vector3Int queryPosition;
            do
            {
                doesFit = true;
                queryPosition = rootPosition + Vector3Int.RoundToInt(direction * tries);

                for (int j = 0; j < i; j++)
                {
                    if (generatedRooms[j].Intersects(new Bounds(queryPosition, size)))
                    {
                        doesFit = false;
                        break;
                    }
                }

                tries++;
            }
            while (!doesFit);

            generatedRooms[i] = new Bounds(queryPosition, size - new Vector3Int(m_RoomSpacing * 2, m_RoomSpacing * 2, 0));
        }

        for (int i = 0; i < generatedRooms.Length; i++)
        {
            Bounds room = generatedRooms[i];

            Debug.Log(room);

            for (int x = 0; x < room.size.x * 2; x++)
            {
                for (int j = 0; j < m_RoomBorder; j++)
                {
                    int y = (int)(x < room.size.x ? j : room.size.y - j);

                    Vector3Int position = new Vector3Int
                    {
                        x = (int)((x % room.size.x) - room.extents.x + room.center.x),
                        y = (int)(y - room.extents.y + room.center.y),
                    };

                    m_WallMap.SetTile(position, m_WallTile);
                }
            }

            for (int y = 0; y < room.size.y * 2; y++)
            {
                for (int j = 0; j < m_RoomBorder; j++)
                {
                    int x = (int)(y < room.size.y ? j : room.size.x - j);

                    Vector3Int position = new Vector3Int
                    {
                        x = (int)(x - room.extents.x + room.center.x),
                        y = (int)((y % room.size.y) - room.extents.y + room.center.y),
                    };

                    m_WallMap.SetTile(position, m_WallTile);
                }
            }
        }
    }
}
