using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    public Transform playerCamera;
    public Sprite[] tileSprites = new Sprite[9];
    public float tileSize = 10f;

    private SpriteRenderer[] renderers = new SpriteRenderer[9];
    private Vector2Int currentCenter = Vector2Int.zero;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject go = new GameObject("Tile_" + (i + 1));
            go.transform.parent = transform;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = tileSprites[i];
            renderers[i] = sr;
        }

        UpdateTilePositions();
    }

    /**@todo
    Update 제거하고, 플레이어가 입력을 받을 때, UpdateBackground 호출하게 하기*/
    private void Update()
    {
        UpdateBackground();
    }

    public void UpdateBackground()
    {
        Vector2 camPos = playerCamera.position;

        int centerX = Mathf.RoundToInt(camPos.x / tileSize);
        int centerY = Mathf.RoundToInt(camPos.y / tileSize);

        Vector2Int offsetInTiles = new Vector2Int(centerX, centerY);

        if (offsetInTiles != currentCenter)
        {
            currentCenter = offsetInTiles;
            UpdateTilePositions();
        }
    }

    private void UpdateTilePositions()
    {
        /**
        Index  Tile
        0 1 2  1 2 3
        3 4 5  4 5 6
        6 7 8  7 8 9
        */

        for (int i = 0; i < 9; i++)
        {
            int dx = (i % 3) - 1;     // -1, 0, 1
            int dy = (i / 3) - 1;     // -1, 0, 1

        Vector2Int tileOffset = new Vector2Int(dx, dy);
        Vector2Int tileIndex = currentCenter + tileOffset;

        Vector3 worldPos = new Vector3(tileIndex.x * tileSize, tileIndex.y * tileSize, 0f);
        renderers[i].transform.position = worldPos;
        }
    }
}
