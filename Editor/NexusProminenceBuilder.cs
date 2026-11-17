// Nexus Prominence (Hank Raid) Playground template builder for MPN SceneTools (Unity 2019.2.6f1).
// Copy this file into <SceneTools>/Assets/Editor/ and run: menu Madness > Templates > Build Nexus Prominence _PG.
// It copies SamplePlayground_PG into NexusProminence_PG, clones the sample room into ten rooms laid out
// after the "NEXUS: HANK RAID" collab (street checkpoint to control tower), chains their doors, paints the
// gray-and-red Nexus look, places Nexus enemies and sets the asset label. Build the .sobj afterwards from
// Madness > AssetBundle as usual. Re-running overwrites the generated scene (asks first).
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class NexusProminenceBuilder
{
    const string SrcScene = "Assets/Scenes/SamplePlayground_PG.unity";
    const string DstScene = "Assets/Scenes/NexusProminence_PG.unity";
    const string MatDir = "Assets/Templates/NexusProminence";
    const string CharStart = "Assets/Library/Prefabs/Markers/Char_Start.prefab";
    const string BundleLabel = "nexusprominence";
    const float RoomSpacing = 300f;

    struct Foe { public CharacterTypes Type; public int Count; public Foe(CharacterTypes t, int n) { Type = t; Count = n; } }

    class RoomSpec
    {
        public string Name;
        public AlertStatus Alert;
        public ProxyObj_Cover.ProxyType[] Cover;
        public Foe[] Foes;
        public bool KeepLocker, KeepSnipers;
        public float Width = 1f, Depth = 1f; // stretch of the sample box: rooms are not all the same square
        public Vector2[] CoverAt;           // cover spots as (0..1 across, 0..1 from front to back); default spread
    }

    static readonly RoomSpec[] Rooms =
    {
        new RoomSpec { Name = "01_StreetCheckpoint", Width = 1.0f, Depth = 0.8f, Alert = AlertStatus.Unaware, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Barrel, ProxyObj_Cover.ProxyType.Crate1 },
            CoverAt = new[] { new Vector2(0.3f, 0.35f), new Vector2(0.7f, 0.5f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new RoomSpec { Name = "02_WantedHall", Width = 1.3f, Depth = 1f, Alert = AlertStatus.Unaware, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Wall1, ProxyObj_Cover.ProxyType.Crate1, ProxyObj_Cover.ProxyType.Barrel },
            CoverAt = new[] { new Vector2(0.35f, 0.4f), new Vector2(0.6f, 0.65f), new Vector2(0.85f, 0.3f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 3), new Foe(CharacterTypes.Nexus2_Scout, 1) } },
        new RoomSpec { Name = "03_ControlsOffice", Width = 1.0f, Depth = 1f, Alert = AlertStatus.Unaware, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Desk1, ProxyObj_Cover.ProxyType.Desk2, ProxyObj_Cover.ProxyType.Desk1 },
            CoverAt = new[] { new Vector2(0.25f, 0.45f), new Vector2(0.55f, 0.3f), new Vector2(0.8f, 0.6f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus3_Support, 1) } },
        new RoomSpec { Name = "04_CargoBay", Width = 1.6f, Depth = 1.15f, Alert = AlertStatus.Hunting, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Crate2, ProxyObj_Cover.ProxyType.Crate3, ProxyObj_Cover.ProxyType.Pallet, ProxyObj_Cover.ProxyType.Crate4, ProxyObj_Cover.ProxyType.Crate1, ProxyObj_Cover.ProxyType.Crate3 },
            CoverAt = new[] { new Vector2(0.15f, 0.6f), new Vector2(0.3f, 0.3f), new Vector2(0.45f, 0.55f), new Vector2(0.6f, 0.25f), new Vector2(0.72f, 0.7f), new Vector2(0.88f, 0.4f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus4_Riot__Guard, 2), new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus6_Soldat, 1) } },
        new RoomSpec { Name = "05_SniperLoft", Width = 1.2f, Depth = 1f, Alert = AlertStatus.Hunting, KeepSnipers = true, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Wall2, ProxyObj_Cover.ProxyType.Crate1, ProxyObj_Cover.ProxyType.Barrel },
            CoverAt = new[] { new Vector2(0.3f, 0.4f), new Vector2(0.6f, 0.6f), new Vector2(0.8f, 0.3f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus2_Scout, 2), new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new RoomSpec { Name = "06_ShutterGarage", Width = 1.35f, Depth = 1f, Alert = AlertStatus.Hunting, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Crate4, ProxyObj_Cover.ProxyType.Barrel, ProxyObj_Cover.ProxyType.Pallet, ProxyObj_Cover.ProxyType.Crate2 },
            CoverAt = new[] { new Vector2(0.2f, 0.4f), new Vector2(0.4f, 0.7f), new Vector2(0.6f, 0.35f), new Vector2(0.82f, 0.6f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus5_Engineer, 2), new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new RoomSpec { Name = "07_SpotlightCorridor", Width = 2.0f, Depth = 0.75f, Alert = AlertStatus.Combat, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Wall1, ProxyObj_Cover.ProxyType.Barrel, ProxyObj_Cover.ProxyType.Wall3 },
            CoverAt = new[] { new Vector2(0.3f, 0.5f), new Vector2(0.55f, 0.5f), new Vector2(0.78f, 0.5f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 4), new Foe(CharacterTypes.Nexus4_Riot__Guard, 1) } },
        new RoomSpec { Name = "08_LockerRoom", Width = 0.85f, Depth = 0.85f, Alert = AlertStatus.Combat, KeepLocker = true, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Wall3, ProxyObj_Cover.ProxyType.Table },
            CoverAt = new[] { new Vector2(0.4f, 0.45f), new Vector2(0.7f, 0.6f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus4_Riot__Guard, 1), new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new RoomSpec { Name = "09_BreakRoom", Width = 1.15f, Depth = 1f, Alert = AlertStatus.Combat, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Table, ProxyObj_Cover.ProxyType.Table, ProxyObj_Cover.ProxyType.Counter, ProxyObj_Cover.ProxyType.Barrel },
            CoverAt = new[] { new Vector2(0.2f, 0.4f), new Vector2(0.45f, 0.55f), new Vector2(0.7f, 0.35f), new Vector2(0.9f, 0.65f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus3_Support, 1), new Foe(CharacterTypes.Nexus1_Agent, 3), new Foe(CharacterTypes.Nexus_Scientist, 1) } },
        new RoomSpec { Name = "10_ControlTower", Width = 1.5f, Depth = 1.1f, Alert = AlertStatus.Combat, 
            Cover = new[] { ProxyObj_Cover.ProxyType.Desk1, ProxyObj_Cover.ProxyType.Counter, ProxyObj_Cover.ProxyType.Desk2, ProxyObj_Cover.ProxyType.Desk1, ProxyObj_Cover.ProxyType.Crate1 },
            CoverAt = new[] { new Vector2(0.2f, 0.35f), new Vector2(0.4f, 0.6f), new Vector2(0.6f, 0.3f), new Vector2(0.78f, 0.55f), new Vector2(0.9f, 0.25f) },
            Foes = new[] { new Foe(CharacterTypes.Nexus6_Soldat, 2), new Foe(CharacterTypes.Nexus4_Riot__Guard, 2), new Foe(CharacterTypes.Nexus5_Engineer, 1), new Foe(CharacterTypes.Nexus1_Agent, 2) } },
    };

    [MenuItem("Madness/Templates/Build Nexus Prominence _PG")]
    public static void Build()
    {
        solidCount = 0; filmStructures.Clear(); tiledMats.Clear(); uvMeshes.Clear(); modelCount = 0; propMeshes.Clear();
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(SrcScene) == null) { Debug.LogError("NexusProminence: " + SrcScene + " not found; pull SceneTools."); return; }
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(DstScene) != null &&
            !Application.isBatchMode && !EditorUtility.DisplayDialog("Nexus Prominence", DstScene + " exists. Overwrite it?", "Overwrite", "Cancel")) return;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        AssetDatabase.DeleteAsset(DstScene);
        AssetDatabase.CopyAsset(SrcScene, DstScene);
        var scene = EditorSceneManager.OpenScene(DstScene, OpenSceneMode.Single);

        AssetDatabase.Refresh();
        var mats = MakeMaterials();
        var charPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharStart);
        var template = Object.FindObjectsOfType<RoomObj>().OrderBy(r => r.transform.position.x).First();
        foreach (var extra in Object.FindObjectsOfType<RoomObj>()) if (extra != template) Object.DestroyImmediate(extra.gameObject);

        frames.Clear();
        var built = new List<RoomObj>();
        // Clone every room from the bare template first, then dress each one; cloning a dressed room
        // would copy its set, enemies and markers into the next one.
        for (int i = 0; i < Rooms.Length; i++)
            built.Add(i == 0 ? template
                : Object.Instantiate(template.gameObject, template.transform.position + Vector3.right * RoomSpacing * i, template.transform.rotation).GetComponent<RoomObj>());
        for (int i = 0; i < Rooms.Length; i++)
        {
            var room = built[i];
            room.gameObject.name = "Room_" + Rooms[i].Name; // before Stretch, so its log lines name the room
            Stretch(room, Rooms[i].Width * WidthScale, Rooms[i].Depth); // owner 2026-11-11: narrower
            FitCamera(room);
            room.ForceStartingRoom = i == 0;
            if (i > 0) StripStartOnly(room);
            else if (!PlaygroundControlRoom) StripControlRoom(room);
            Dress(room, Rooms[i], i, mats, charPrefab);
        }
        ChainDoors(built);
        if (!PlaygroundControlRoom) PlacePlayerStart(built[0], frames[built[0]]);
        // after chaining, so the spawn-only doors are never picked as a room's way in or out
        for (int i = 0; i < built.Count; i++) Encounters(built[i], frames[built[i]], i);
        TileUVs(built);
        CheckFilmStructures(built);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        var importer = AssetImporter.GetAtPath(DstScene);
        importer.SetAssetBundleNameAndVariant(BundleLabel, "");
        AssetDatabase.SaveAssets();
        Debug.Log("NexusProminence: " + solidCount + " solid props (Obstacle colliders)");
        Debug.Log("NexusProminence: " + modelCount + " modelled props");
        Debug.Log("NexusProminence: built " + built.Count + " rooms into " + DstScene + " (label '" + BundleLabel + "'). Select Room_01_StreetCheckpoint, press SYNCH VISUALS, check it, then run scene-lint and the AssetBundle build.");
    }

    // Stretch the cloned sample box: room meshes scale about the left wall (x) and the back wall (z);
    // everything else that sits in the room (doors, cover, markers, lights, camera borders, camera) moves
    // with the stretch but keeps its size. The camera keeps its distance (only x moves).
    static void Stretch(RoomObj room, float sx, float sz)
    {
        if (Mathf.Approximately(sx, 1f) && Mathf.Approximately(sz, 1f)) return;
        Physics.SyncTransforms();
        var fb = FloorBounds(room);
        float ax = fb.min.x, az = fb.max.z;
        int scaled = 0, moved = 0;
        StretchWalk(room.transform, room, ax, az, sx, sz, ref scaled, ref moved);
        if (room.CameraPoint != null)
        {
            var t = room.CameraPoint.transform; var p = t.position;
            t.position = new Vector3(ax + (p.x - ax) * sx, p.y, p.z);
        }
        Debug.Log("NexusProminence: stretched " + room.name + " x" + sx + " z" + sz + " (" + scaled + " meshes scaled, " + moved + " objects moved)");
    }

    // Cam_Main (decompile, build 25615196) follows the player inside [border + m, border - m] where
    // m = |target - camera| * fov * 0.005 * (aspect - 1) once the room is over 17 units wide, and follows
    // depth only by Leeway z (0 in the sample room: the camera never moves toward the front). With the
    // borders on the walls a player in a front corner walked off screen. So: follow depth fully and set
    // the side borders so the camera centre can reach EdgeLead units past a player standing at a wall.
    const float EdgeLead = 10f;
    static void FitCamera(RoomObj room)
    {
        if (room.CameraPoint == null || room.CameraTarget == null || room.BorderLeft == null || room.BorderRight == null) return;
        Physics.SyncTransforms();
        var fb = FloorBounds(room);
        var offset = room.CameraTarget.transform.position - room.CameraPoint.transform.position;
        float m = offset.magnitude * room.CameraPoint.fieldOfView * 0.005f * (16f / 9f - 1f);
        bool leftIsMin = room.BorderLeft.transform.position.x < room.BorderRight.transform.position.x;
        var lo = leftIsMin ? room.BorderLeft.transform : room.BorderRight.transform;
        var hi = leftIsMin ? room.BorderRight.transform : room.BorderLeft.transform;
        lo.position = new Vector3(fb.min.x + 1.5f + EdgeLead - m, lo.position.y, lo.position.z);
        hi.position = new Vector3(fb.max.x - 1.5f - EdgeLead + m, hi.position.y, hi.position.z);
        if (lo.position.x > hi.position.x) { float c = fb.center.x; lo.position = new Vector3(c - 9f, lo.position.y, lo.position.z); hi.position = new Vector3(c + 9f, hi.position.y, hi.position.z); }
        room.Leeway_Pos = new Vector3(room.Leeway_Pos.x, room.Leeway_Pos.y, DepthFollow);
        room.Leeway_Look = new Vector3(room.Leeway_Look.x, room.Leeway_Look.y, DepthFollow);
        if (room.BorderBack != null && room.BorderForward != null)
        {
            bool backIsMin = room.BorderBack.transform.position.z < room.BorderForward.transform.position.z;
            var zlo = backIsMin ? room.BorderBack.transform : room.BorderForward.transform;
            var zhi = backIsMin ? room.BorderForward.transform : room.BorderBack.transform;
            zlo.position = new Vector3(zlo.position.x, zlo.position.y, fb.min.z);
            zhi.position = new Vector3(zhi.position.x, zhi.position.y, fb.max.z);
        }
        Debug.Log("NexusProminence: camera fit " + room.name + " margin " + m.ToString("0.0") + " x " + lo.position.x.ToString("0.0") + ".." + hi.position.x.ToString("0.0"));
    }
    // owner 2026-11-11: narrower rooms, one small step (0.6 together with depth and zoom changes broke the map)
    const float WidthScale = 0.85f;
    const float DepthFollow = 0.6f;

    static void StretchWalk(Transform parent, RoomObj room, float ax, float az, float sx, float sz, ref int scaled, ref int moved)
    {
        foreach (Transform c in parent)
        {
            if (room.CameraPoint != null && c == room.CameraPoint.transform) continue;
            var p = c.position;
            var np = new Vector3(ax + (p.x - ax) * sx, p.y, az + (p.z - az) * sz);
            bool geometry = (c.GetComponent<MeshRenderer>() != null || c.GetComponent<Collider>() != null) && c.GetComponent<SceneObj>() == null && c.GetComponent<Camera>() == null && c.childCount == 0;
            if (geometry)
            {
                c.position = np;
                var s = c.localScale;
                // imported meshes are often rotated (Blender's -90 on X): scale each local axis by the
                // stretch of the world axis it lines up with
                System.Func<Vector3, float> k = d => Mathf.Abs(d.x) > 0.7f ? sx : Mathf.Abs(d.z) > 0.7f ? sz : 1f;
                c.localScale = new Vector3(s.x * k(c.right), s.y * k(c.up), s.z * k(c.forward));
                scaled++;
            }
            else if (c.GetComponent<SceneObj>() != null || c.GetComponent<Light>() != null || c.GetComponent<Camera>() != null || c.childCount == 0)
            {
                c.position = np; moved++;
            }
            else StretchWalk(c, room, ax, az, sx, sz, ref scaled, ref moved);
        }
    }

    // Round 2 (build zl): the sample's PX_Playground proxy turns in game into the Playground control room
    // (Converter_Proxy.ReplaceProxy: Library.Proxy_Playground, window masked through the back wall), a white
    // lab booth showing through room 1's city skyline: the Workshop's "playground room for no reason". The
    // map loads without it (it is loaded from the Default Chamber's terminal; difficulty is global; the pause
    // menu still returns to the menu). True puts the booth back in room 1 with its spawn and stage menus.
    const bool PlaygroundControlRoom = false;

    static void StripControlRoom(RoomObj room)
    {
        foreach (var p in room.GetComponentsInChildren<ProxyObj_Playground>(true)) Object.DestroyImmediate(p.gameObject);
        Debug.Log("NexusProminence: " + room.name + " has no Playground control room (PlaygroundControlRoom off)");
    }

    // Owner test 2026-11-14 (build zs): the player fell through the world on load. The sample's Player_Start
    // stands inside the Playground booth, 6 m behind room 1's back wall (world z 13.2 vs back wall 6.81) and 7 m
    // up; with the booth gone (round 2) there was nothing under it. Put it on room 1's floor at the left
    // (entry) side, facing into the room, and fail the build if no Floor collider is under it.
    static void PlacePlayerStart(RoomObj room, Frame f)
    {
        var p = room.GetComponentsInChildren<Marker_Player>(true).FirstOrDefault();
        if (p == null) throw new System.Exception("NexusProminence: room 1 has no Player_Start");
        p.transform.position = new Vector3(f.x0 + 2.5f, f.y + 0.05f, (f.zf + f.zb) / 2f);
        p.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        EditorUtility.SetDirty(p.transform);
        Physics.SyncTransforms();
        RaycastHit h;
        bool ok = Physics.Raycast(p.transform.position + Vector3.up * 1f, Vector3.down, out h, 3f) && h.collider.CompareTag("Floor");
        if (!ok) throw new System.Exception("NexusProminence: no Floor under Player_Start at " + p.transform.position);
        Debug.Log("NexusProminence: Player_Start on the floor at " + p.transform.position.ToString("F2") + " (hit " + h.collider.name + ")");
    }

    // Player_Start and the Playground terminal live only in the first room.
    static void StripStartOnly(RoomObj room)
    {
        foreach (var p in room.GetComponentsInChildren<Marker_Player>(true)) Object.DestroyImmediate(p.gameObject);
        foreach (var p in room.GetComponentsInChildren<ProxyObj_Playground>(true)) Object.DestroyImmediate(p.gameObject);
    }

    static Dictionary<string, Material> MakeMaterials()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Templates")) AssetDatabase.CreateFolder("Assets", "Templates");
        if (!AssetDatabase.IsValidFolder(MatDir)) AssetDatabase.CreateFolder("Assets/Templates", "NexusProminence");
        var d = new Dictionary<string, Material>
        {
            { "wall", Mat("NX_Wall", LighterWalls ? new Color(0.40f, 0.35f, 0.36f) : new Color(0.25f, 0.20f, 0.21f), Color.black) },
            { "floor", Mat("NX_Floor", LighterWalls ? new Color(0.52f, 0.47f, 0.48f) : new Color(0.34f, 0.29f, 0.30f), Color.black) },
            { "panel", Mat("NX_Panel", LighterWalls ? new Color(0.46f, 0.41f, 0.42f) : new Color(0.31f, 0.26f, 0.27f), Color.black) },
            { "red", Mat("NX_RedStripe", new Color(0.85f, 0.04f, 0.04f), new Color(0.55f, 0f, 0f)) },
            { "maroon", Mat("NX_Maroon", new Color(0.50f, 0.03f, 0.05f), new Color(0.30f, 0f, 0.02f)) },
            { "rubble", Mat("NX_Rubble", new Color(0.40f, 0.37f, 0.37f), Color.black) },
            { "dark", Mat("NX_DarkTrim", new Color(0.11f, 0.10f, 0.11f), Color.black) },
            { "black", Mat("NX_Black", new Color(0.02f, 0.02f, 0.02f), Color.black) },
            { "steel", Mat("NX_Steel", new Color(0.52f, 0.51f, 0.52f), Color.black) },
            { "screen", Mat("NX_Screen", new Color(0.9f, 0.1f, 0.1f), new Color(1.0f, 0.12f, 0.12f)) },
            { "sky", Mat("NX_Sky", new Color(0.75f, 0.05f, 0.05f), new Color(0.65f, 0.03f, 0.03f)) },
            { "lit", Mat("NX_LitWindow", new Color(1f, 0.8f, 0.8f), new Color(1f, 0.7f, 0.7f)) },
            { "asphalt", Mat("NX_Asphalt", new Color(0.22f, 0.21f, 0.22f), Color.black) },
            { "stain", Mat("NX_Stain", new Color(0.12f, 0.11f, 0.11f), Color.black) },
            { "white", Mat("NX_White", new Color(0.86f, 0.86f, 0.86f), Color.black) },
            { "glass", Mat("NX_Glass", new Color(0.55f, 0.08f, 0.10f), new Color(0.45f, 0.05f, 0.08f)) },
            { "beam", BeamMat() },
        };
        return d;
    }

    static Material Mat(string name, Color c, Color emission)
    {
        string path = MatDir + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        // Unlit-looking surfaces (no emission) use the game's own toon terrain shader, so they get the black
        // Madness outline and match the vanilla cover; it reads its colour from _MainTex, so each gets a
        // small solid texture. Glowing pieces keep Standard (the Madness terrain shader has no emission).
        var mad = Shader.Find(MadShader);
        bool useMad = emission == Color.black && mad != null;
        if (m == null) { m = new Material(useMad ? mad : Shader.Find("Standard")); AssetDatabase.CreateAsset(m, path); }
        if (useMad)
        {
            m.shader = mad;
            var tile = TexturedSurfaces ? TileTex(name) : null;
            m.mainTexture = tile != null ? tile : SolidTex(name, c);
            m.color = Color.white;
            if (m.HasProperty("_Outline")) m.SetFloat("_Outline", 0.06f);
            if (m.HasProperty("_OutlineColor")) m.SetColor("_OutlineColor", Color.black);
            if (m.HasProperty("_TintColor")) m.SetColor("_TintColor", new Color(0, 0, 0, 0));
            if (tile != null) tiledMats.Add(m);
            EditorUtility.SetDirty(m);
            return m;
        }
        if (mad == null) Debug.LogWarning("NexusProminence: shader " + MadShader + " not found; using Standard");
        m.shader = Shader.Find("Standard");
        m.color = c;
        m.SetFloat("_Glossiness", 0.08f);
        if (emission != Color.black) { m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", emission); m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None; }
        else { m.DisableKeyword("_EMISSION"); m.SetColor("_EmissionColor", Color.black); }
        EditorUtility.SetDirty(m);
        return m;
    }

    const string MadShader = "Madness/Terrain - General";

    // Round 3 (build zm): drawn tiling textures on the main surfaces (wall panels, floor slabs) instead of a
    // solid 4x4 colour. Tiles come from tools/make_textures.py (Z-Image render, cropped to whole cells, tinted
    // to the colours above) into MatDir/Tex; missing tiles fall back to the solid colour. Meshes get UVs in
    // metres (TileMetres per repeat) so a tile keeps its size on every box. false = the zl look.
    const bool TexturedSurfaces = true;
    const float TileMetres = 2.5f;
    static readonly Dictionary<string, string> TileFor = new Dictionary<string, string>
        { { "NX_Wall", "tile_wall" }, { "NX_Panel", "tile_panel" }, { "NX_Floor", "tile_floor" }, { "NX_Asphalt", "tile_asphalt" } };
    static readonly HashSet<Material> tiledMats = new HashSet<Material>();

    static Texture2D TileTex(string matName)
    {
        string tex; if (!TileFor.TryGetValue(matName, out tex)) return null;
        if (LighterWalls) tex += "_light";
        string tpath = MatDir + "/Tex/" + tex + ".png";
        var ti = AssetImporter.GetAtPath(tpath) as TextureImporter;
        if (ti == null) { Debug.LogWarning("NexusProminence: missing " + tpath + " (run tools/make_textures.py); solid colour used"); return null; }
        if (ti.wrapMode != TextureWrapMode.Repeat || !ti.mipmapEnabled || ti.maxTextureSize != 512)
        { ti.wrapMode = TextureWrapMode.Repeat; ti.mipmapEnabled = true; ti.maxTextureSize = 512; ti.SaveAndReimport(); }
        return AssetDatabase.LoadAssetAtPath<Texture2D>(tpath);
    }

    // Box-projected UVs in metres for every renderer that wears a tiled material: each vertex takes the two
    // axes its normal does not point along, scaled by the object's own scale, so a 2 m crate and a 20 m wall
    // show panels of the same size. Meshes are cached per (source mesh, scale) as assets in MatDir/UV.
    static readonly Dictionary<string, Mesh> uvMeshes = new Dictionary<string, Mesh>();
    static void TileUVs(IEnumerable<RoomObj> rooms)
    {
        if (!TexturedSurfaces) return;
        if (!AssetDatabase.IsValidFolder(MatDir + "/UV")) AssetDatabase.CreateFolder(MatDir, "UV");
        int n = 0;
        foreach (var room in rooms)
            foreach (var r in room.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (r.sharedMaterial == null || !tiledMats.Contains(r.sharedMaterial)) continue;
                var mf = r.GetComponent<MeshFilter>(); if (mf == null || mf.sharedMesh == null) continue;
                var src = mf.sharedMesh; if (src.name.StartsWith("NX_UV_")) continue;
                var sc = r.transform.lossyScale;
                var q = new Vector3(Mathf.Round(sc.x * 4) / 4, Mathf.Round(sc.y * 4) / 4, Mathf.Round(sc.z * 4) / 4);
                string key = src.name + "_" + q.x + "x" + q.y + "x" + q.z;
                Mesh m;
                if (!uvMeshes.TryGetValue(key, out m) || m == null)
                {
                    string path = MatDir + "/UV/NX_UV_" + uvMeshes.Count + ".asset";
                    m = Object.Instantiate(src); m.name = "NX_UV_" + key;
                    var v = src.vertices; var nrm = src.normals; var uv = new Vector2[v.Length];
                    for (int i = 0; i < v.Length; i++)
                    {
                        var p = Vector3.Scale(v[i], q) / TileMetres;
                        var a = nrm.Length > i ? nrm[i] : Vector3.up; a = new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
                        uv[i] = a.x >= a.y && a.x >= a.z ? new Vector2(p.z, p.y) : a.y >= a.z ? new Vector2(p.x, p.z) : new Vector2(p.x, p.y);
                    }
                    m.uv = uv;
                    AssetDatabase.DeleteAsset(path); AssetDatabase.CreateAsset(m, path);
                    uvMeshes[key] = m;
                }
                mf.sharedMesh = m; n++;
            }
        Debug.Log("NexusProminence: tiled UVs on " + n + " renderers, " + uvMeshes.Count + " meshes");
    }

    static Texture2D SolidTex(string name, Color c)
    {
        string path = MatDir + "/" + name + "_Tex.asset";
        var t = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (t == null) { t = new Texture2D(4, 4, TextureFormat.RGBA32, false); AssetDatabase.CreateAsset(t, path); }
        var px = new Color[16]; for (int i = 0; i < 16; i++) px[i] = c;
        t.SetPixels(px); t.Apply(); EditorUtility.SetDirty(t);
        return t;
    }

    // Soft red light cone under the ceiling lamps (additive, so it brightens what is behind it).
    static Material BeamMat()
    {
        string path = MatDir + "/NX_Beam.mat";
        var sh = Shader.Find("Legacy Shaders/Particles/Additive") ?? Shader.Find("Particles/Additive") ?? Shader.Find("Standard");
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null) { m = new Material(sh); AssetDatabase.CreateAsset(m, path); }
        m.shader = sh;
        if (m.HasProperty("_TintColor")) m.SetColor("_TintColor", new Color(0.55f, 0.04f, 0.04f, 0.5f));
        else m.color = new Color(0.6f, 0.05f, 0.05f);
        EditorUtility.SetDirty(m);
        return m;
    }

    static Mesh beamMesh;
    static Mesh BeamMesh()
    {
        if (beamMesh != null) return beamMesh;
        string path = MatDir + "/NX_BeamMesh.asset";
        beamMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (beamMesh != null) return beamMesh;
        beamMesh = new Mesh { name = "NX_BeamMesh" };
        // Trapezoid in XY: narrow top at y=1, wide bottom at y=0; both faces.
        beamMesh.vertices = new[] { new Vector3(-0.15f, 1, 0), new Vector3(0.15f, 1, 0), new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0) };
        beamMesh.uv = new[] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0) };
        beamMesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 2, 1, 0, 3, 2, 0 };
        beamMesh.RecalculateNormals();
        beamMesh.RecalculateBounds();
        AssetDatabase.CreateAsset(beamMesh, path);
        return beamMesh;
    }

    // zzh (owner's glitch shot 3: "flat cards with no depth"): the lamp cone as a real volume. A frustum (top radius
    // 0.15, bottom 0.5, unit height) whose vertex colours are brightest at the silhouette sides and the lamp and
    // fade toward the floor, so the additive shader (Cull Off, both walls drawn) reads as a lit cone from the game
    // camera, plus a soft pool on the floor (bright centre fading to nothing at 0.75) 0.013 up the unit height, above
    // every paper and splat. Rebuilt each build. Same renderer as the old card, so shooting the lamp still hides it.
    static Mesh coneMesh;
    static Mesh ConeMesh()
    {
        if (coneMesh != null) return coneMesh;
        string path = MatDir + "/NX_ConeMesh.asset";
        if (AssetDatabase.LoadAssetAtPath<Mesh>(path) != null) AssetDatabase.DeleteAsset(path);
        const int N = 32; float[] ys = { 1f, 0.7f, 0.4f, 0f };
        var v = new List<Vector3>(); var c = new List<Color>(); var tri = new List<int>();
        for (int j = 0; j < ys.Length; j++)
            for (int i = 0; i <= N; i++)
            {
                float a = i * Mathf.PI * 2f / N, r = Mathf.Lerp(0.5f, 0.15f, ys[j]);
                v.Add(new Vector3(Mathf.Sin(a) * r, ys[j], -Mathf.Cos(a) * r));
                float rim = 0.25f + 0.75f * Mathf.Pow(Mathf.Abs(Mathf.Sin(a)), 2f), k = 0.55f * rim * Mathf.Lerp(0.3f, 1f, ys[j]);
                c.Add(new Color(k, k, k, k));
            }
        for (int j = 0; j + 1 < ys.Length; j++)
            for (int i = 0; i < N; i++)
            {
                int a = j * (N + 1) + i, b = a + N + 1;
                tri.AddRange(new[] { a, b, a + 1, a + 1, b, b + 1 });
            }
        float py = 0.013f; int ctr = v.Count;
        v.Add(new Vector3(0f, py, 0f)); c.Add(new Color(0.8f, 0.8f, 0.8f, 0.8f));
        float[] rings = { 0.45f, 0.75f }, glow = { 0.4f, 0f };
        for (int q = 0; q < 2; q++)
            for (int i = 0; i <= N; i++)
            {
                float a = i * Mathf.PI * 2f / N;
                v.Add(new Vector3(Mathf.Sin(a) * rings[q], py, -Mathf.Cos(a) * rings[q])); c.Add(new Color(glow[q], glow[q], glow[q], glow[q]));
            }
        int r0 = ctr + 1, r1 = r0 + N + 1;
        for (int i = 0; i < N; i++)
        {
            tri.AddRange(new[] { ctr, r0 + i + 1, r0 + i });
            tri.AddRange(new[] { r0 + i, r0 + i + 1, r1 + i, r0 + i + 1, r1 + i + 1, r1 + i });
        }
        coneMesh = new Mesh { name = "NX_ConeMesh" };
        coneMesh.SetVertices(v); coneMesh.SetColors(c); coneMesh.SetTriangles(tri, 0);
        coneMesh.uv = v.Select(q => new Vector2(q.x + 0.5f, q.y)).ToArray();
        coneMesh.RecalculateNormals(); coneMesh.RecalculateBounds();
        AssetDatabase.CreateAsset(coneMesh, path);
        return coneMesh;
    }

    static Bounds FloorBounds(RoomObj room)
    {
        var floors = room.GetComponentsInChildren<Collider>(true).Where(c => c.CompareTag("Floor")).ToArray();
        if (floors.Length == 0) floors = room.GetComponentsInChildren<Collider>(true);
        var b = floors[0].bounds;
        foreach (var c in floors) b.Encapsulate(c.bounds);
        return b;
    }

    // Room frame shared by every room: x0..x1 side walls, zb back wall, zf the open (camera) side, y floor top.
    class Frame
    {
        public Transform Parent; public float x0, x1, zb, zf, y; public Dictionary<string, Material> M;
        public List<Vector3> Doors = new List<Vector3>();
        public float W { get { return x1 - x0; } }
        public float D { get { return zb - zf; } }
        public float Cx { get { return (x0 + x1) * 0.5f; } }
        public float Cz { get { return (zb + zf) * 0.5f; } }
    }

    static void Dress(RoomObj room, RoomSpec spec, int index, Dictionary<string, Material> mats, GameObject charPrefab)
    {
        // Look (from the video frames): charcoal walls, light gray floor, bright red band and skirting,
        // red seams where floor meets wall, plain white light. No fog: the video's red cast is its colour grade.
        foreach (var r in room.GetComponentsInChildren<MeshRenderer>(true))
        {
            if (r.GetComponentInParent<SceneObj>() != null && r.GetComponentInParent<SceneObj>() != room) continue; // leave proxies and markers
            var col = r.GetComponent<Collider>();
            bool isFloor = col != null && col.CompareTag("Floor");
            r.sharedMaterial = isFloor ? mats["floor"] : mats["wall"];
        }
        room.AmbientColor = new Color(0.33f, 0.27f, 0.27f); // warm maroon cast, as in the film
        room.FogDensity = 0f;
        room.FogColor = Color.black;
        if (room.CameraPoint != null)
            foreach (var gf in room.CameraPoint.GetComponents<MonoBehaviour>().Where(b => b != null && b.GetType().Name == "GlobalFog")) gf.enabled = false;
        foreach (var l in room.GetComponentsInChildren<Light>(true)) { l.color = new Color(1f, 0.90f, 0.88f); }

        Physics.SyncTransforms();
        var fb = FloorBounds(room);
        var layout = room.transform.Find("Layout") ?? room.transform;
        var setGo = new GameObject("NX_Set");
        setGo.transform.SetParent(layout, false);
        setGo.transform.position = new Vector3(fb.center.x, fb.max.y, fb.center.z);
        setGo.isStatic = true;
        var f = new Frame { Parent = setGo.transform, x0 = fb.min.x, x1 = fb.max.x, zb = fb.max.z, zf = fb.min.z, y = fb.max.y, M = mats };
        // The floor runs under the walls: find the walls' inner faces by raycasting out from the middle of the room.
        Physics.SyncTransforms();
        var walls = room.GetComponentsInChildren<Collider>(true).Where(c => !c.CompareTag("Floor")).ToArray();
        var origin = new Vector3(fb.center.x, f.y + 1.2f, fb.center.z);
        f.zb = WallHit(walls, origin, Vector3.forward, f.zb);
        f.x0 = WallHit(walls, origin, Vector3.left, f.x0);
        f.x1 = WallHit(walls, origin, Vector3.right, f.x1);
        Debug.Log("NexusProminence: " + room.name + " walls x " + f.x0 + ".." + f.x1 + " back z " + f.zb + " (floor " + fb.min.x + ".." + fb.max.x + ", " + fb.max.z + ")");

        f.Doors = room.GetComponentsInChildren<ProxyObj_Door>(true).Select(d => d.transform.position).ToList();
        frames[room] = f;
        BuildMapWalls(f, room);
        Shell(f, room, index);
        curRoom = room;
        switch (index)
        {
            case 0: Street(f); break;
            case 1:
                WantedHall(f);
                // The video grades this hall deep red: tint the room's own light and ambient.
                foreach (var l in room.GetComponentsInChildren<Light>(true)) l.color = new Color(1f, 0.42f, 0.42f);
                room.AmbientColor = new Color(0.55f, 0.16f, 0.16f);
                break;
            case 2: ControlsOffice(f); break;
            case 3: CargoBay(f); break;
            case 4: SniperLoft(f); break;
            case 5: ShutterGarage(f); break;
            case 6: SpotlightCorridor(f); break;
            case 7: LockerRoom(f); break;
            case 8: BreakRoom(f); break;
            case 9: ControlTower(f); break;
        }
        Identity(f, room, index);
        ClearWallOverlaps(f, room);
        LightTheLamps(room, f);
        IdentityLights(room, index);
        BoxFourthWall(room);
        Physics.SyncTransforms();
        CollectWallBusy(f, room);
        Stations(f, room, index);
        Gags(f, index);
        Debris(f, index);

        // Cover: reuse the room's PX_Cover objects, add or remove to match the spec.
        var covers = room.GetComponentsInChildren<ProxyObj_Cover>(true).ToList();
        if (covers.Count > 0)
        {
            while (covers.Count < spec.Cover.Length)
                covers.Add(Object.Instantiate(covers[0].gameObject, covers[0].transform.parent).GetComponent<ProxyObj_Cover>());
            for (int c = 0; c < covers.Count; c++)
            {
                if (c >= spec.Cover.Length) { Object.DestroyImmediate(covers[c].gameObject); continue; }
                covers[c].Type = spec.Cover[c];
                var p = covers[c].transform.position;
                // Spread through the floor, not one row: the video's rooms have crates and desks out in the open.
                Vector2 at = FilmRoom6Structures && index == 5 && c == 3 ? new Vector2(0.92f, 0.6f) // clear of the near pylon (zw)
                    : FilmRoom5Structures && index == 4 && c == 1 ? new Vector2(0.88f, 0.55f) // clear of the sloped wall and the stack (zx)
                    : spec.CoverAt != null && c < spec.CoverAt.Length ? spec.CoverAt[c]
                    : new Vector2((c + 1f) / (spec.Cover.Length + 1f), c % 2 == 0 ? 0.35f : 0.65f);
                var cp = new Vector3(Mathf.Lerp(f.x0 + 1.5f, f.x1 - 1.5f, at.x), p.y, Mathf.Lerp(fb.min.z + 2f, f.zb - 1.5f, at.y));
                // keep cover out of doorways (Workshop feedback: cover in front of a door): push it toward the
                // middle of the room until it is 4 units from every door
                for (int guard = 0; guard < 20 && f.Doors.Any(d => new Vector2(d.x - cp.x, d.z - cp.z).magnitude < 4f); guard++)
                    cp.x += cp.x < f.Cx ? 0.5f : -0.5f;
                covers[c].transform.position = cp;
                covers[c].gameObject.name = "PX_Cover_" + spec.Cover[c];
                EditorUtility.SetDirty(covers[c]);
            }
        }
        if (!spec.KeepLocker) foreach (var l in room.GetComponentsInChildren<ProxyObj_Locker>(true)) Object.DestroyImmediate(l.gameObject);
        if (!spec.KeepSnipers) foreach (var s in room.GetComponentsInChildren<Marker_Waypoint>(true).Where(w => w.gameObject.name.StartsWith("Sniperpoint"))) Object.DestroyImmediate(s.gameObject);

        // Enemies: Nexus faction, spread across the floor right of centre (the player enters from the left).
        if (charPrefab == null) { Debug.LogWarning("NexusProminence: " + CharStart + " missing; no enemies placed."); return; }
        var folder = room.CharacterFolder != null ? room.CharacterFolder.transform : room.transform;
        var foes = StagedFights && index < FilmCountFoes.Length && FilmCountFoes[index] != null ? FilmCountFoes[index] : spec.Foes;
        if (FilmTotals && FilmTotalFoes[index] != null) foes = FilmTotalFoes[index];
        int total = foes.Sum(foe => foe.Count), n = 0;
        foreach (var foe in foes)
            for (int k = 0; k < foe.Count; k++, n++)
            {
                var go = (GameObject)PrefabUtility.InstantiatePrefab(charPrefab, room.gameObject.scene);
                go.transform.SetParent(folder, false);
                // Staged like the video: a loose group right of centre, staggered front to back.
                float t = total == 1 ? 0.7f : Mathf.Lerp(0.4f, 0.92f, n / (float)(total - 1));
                float depth = new[] { 0.5f, 0.25f, 0.75f, 0.4f, 0.6f }[n % 5];
                go.transform.position = new Vector3(Mathf.Lerp(f.x0 + 1f, f.x1 - 1f, t), fb.max.y, Mathf.Lerp(fb.min.z + 2f, f.zb - 1.5f, depth));
                var m = go.GetComponent<Marker_Character>();
                m.Character = foe.Type;
                m.Faction = Factions.Nexus;
                m.Alertness = spec.Alert;
                m.SerialNumber = "NXP_" + spec.Name + "_" + n;
                Loadout(m, foe.Type, index, n);
                if (StagedFights) Stage(go, m, f, fb, index, n);
                go.name = "Char_" + foe.Type + "_" + n;
                EditorUtility.SetDirty(m);
            }
        EditorUtility.SetDirty(room);
    }

    // ---- Round 6 (build zp): every room had one fight, a loose group right of centre facing nobody in
    // particular (concern 4, second pass). StagedFights gives each room its own fight from the film and
    // ludowright (levels-and-space rules 8, 14, 15; one idea per room): where each enemy stands, which way it
    // faces, its alertness, its weapon and one game behaviour. Everything is a field the game's own
    // Marker_Character already carries (decompiled Converter_Marker / Char_DataAssigner): the marker's rotation
    // is the spawn facing, AddBehaviors adds BehaviorList entries (Camper runs M_SniperCamp, which walks to the
    // room's Sniper_Nodes; HoldCover, Defensive, Hunter, Runner), SpawnFromDoor makes a placed enemy walk in
    // through a door. Counts never exceed the film table (rooms 6, 9, 10 were above it and come down);
    // positions are fractions of the room (u across from the entry, v front to back), never nearer the
    // entry than u 0.45 (rule 15: no enemy in melee range of the way in). Set StagedFights false for round 5.
    const bool StagedFights = true;
    enum Face { Entry, Exit, Back, Front }
    struct Post
    {
        public float u, v; public Face face; public string held; public AlertStatus? alert; public BehaviorList[] add;
        public Post(float u, float v, Face face, string held = null, AlertStatus? alert = null, params BehaviorList[] add)
        { this.u = u; this.v = v; this.face = face; this.held = held; this.alert = alert; this.add = add; }
    }
    static readonly Foe[][] FilmCountFoes =
    {
        null, null, null, null, null,
        new[] { new Foe(CharacterTypes.Nexus5_Engineer, 2), new Foe(CharacterTypes.Nexus1_Agent, 1) }, // 6: film about 3 (was 4)
        null, null,
        new[] { new Foe(CharacterTypes.Nexus3_Support, 1), new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus_Scientist, 1) }, // 9: film about 4 (was 5)
        new[] { new Foe(CharacterTypes.Nexus6_Soldat, 2), new Foe(CharacterTypes.Nexus4_Riot__Guard, 1), new Foe(CharacterTypes.Nexus5_Engineer, 1) }, // 10: placed 4 (was 7); the running fight comes in by door and wave
    };
    // Round 7 (build zq), owner rule "don't betray the movie": rooms 2, 4, 7 and 10 had more enemies placed plus
    // backup than the film shows (zp totals 7, 11, 12, 17 against the film's about 4-5, 7, 4-5 and 5; film table in
    // ai-docs/HANDOFF.md, tower re-read at 10:35-10:50: three dead on the floor and two standing). FilmTotals brings
    // each total (placed + every wave) inside the film: room 2 places three and its scout walks in mid-fight as a
    // one-man door squad on the alarm (film 1:05) instead of a placed SpawnFromDoor marker; room 4 keeps its door
    // wave (two agents) and drops the rappel team; room 7 places two agents and the riot guard, two agents step out
    // of the side doors, no vents; room 10 places four and one soldat follows through the door, no tower wave.
    // false = the zp counts.
    const bool FilmTotals = true;
    static readonly Foe[][] FilmTotalFoes =
    {
        null,
        new[] { new Foe(CharacterTypes.Nexus1_Agent, 3) },                                                   // 2: 3 placed + 1 walk-in = 4
        null, null, null, null,
        new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus4_Riot__Guard, 1) },   // 7: 3 placed + 2 door = 5
        null, null, null,
    };
    static readonly Post[][] FilmTotalPosts =
    {
        null, null, null, null, null, null,
        new[] { new Post(0.5f, 0.3f, Face.Entry), new Post(0.82f, 0.35f, Face.Entry, null, null, BehaviorList.Runner), new Post(0.6f, 0.5f, Face.Entry, null, null, BehaviorList.Defensive) },
        null, null, null,
    };
    // door waves under FilmTotals (null = no wave): 2 one scout on the alarm, 4 two agents, 7 two agents, 10 one soldat
    static readonly Foe[][] FilmTotalWaves =
    {
        null, new[] { new Foe(CharacterTypes.Nexus2_Scout, 1) }, null, new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) }, null, null,
        new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) }, null, null, new[] { new Foe(CharacterTypes.Nexus6_Soldat, 1) },
    };
    const AlertStatus Calm = AlertStatus.Unaware;
    static readonly Post[][] Posts =
    {
        // 1 the ambush: two rifle guards at the far barrier, backs to the way in; the player gets the first shot
        new[] { new Post(0.78f, 0.45f, Face.Exit), new Post(0.86f, 0.62f, Face.Exit) },
        // 2 the walk-in: three line the poster wall facing the way in; the scout (index 3) walks in through a side door
        new[] { new Post(0.55f, 0.75f, Face.Entry), new Post(0.7f, 0.8f, Face.Entry), new Post(0.85f, 0.7f, Face.Entry), new Post(0.75f, 0.4f, Face.Entry, null, null, BehaviorList.Hunter) },
        // 3 behind the desks: each at a desk facing the screens (back wall), then holding the desk as cover
        new[] { new Post(0.55f, 0.42f, Face.Back, null, null, BehaviorList.HoldCover), new Post(0.8f, 0.72f, Face.Back, null, null, BehaviorList.HoldCover), new Post(0.6f, 0.62f, Face.Back) },
        // 4 the shield wall: two riot guards in front as blockers, agents and the soldat behind on the crates
        new[] { new Post(0.5f, 0.45f, Face.Entry, null, null, BehaviorList.Defensive), new Post(0.52f, 0.7f, Face.Entry, null, null, BehaviorList.Defensive),
                new Post(0.7f, 0.35f, Face.Entry, null, null, BehaviorList.HoldCover), new Post(0.76f, 0.75f, Face.Entry, null, null, BehaviorList.HoldCover), new Post(0.9f, 0.55f, Face.Entry, "R_Hrifle_NFAL", null, BehaviorList.HoldCover) },
        // 5 the sniper: one scout with the battle rifle camps the loft's sniper points, one scout rushes with the sword, two agents hold low cover
        new[] { new Post(0.85f, 0.8f, Face.Entry, "R_Hrifle_NFAL", null, BehaviorList.Camper, BehaviorList.Sniper), new Post(0.6f, 0.5f, Face.Entry, "M_SwordNexus", null, BehaviorList.Hunter, BehaviorList.Runner),
                new Post(0.65f, 0.32f, Face.Entry, null, null, BehaviorList.HoldCover), new Post(0.78f, 0.62f, Face.Entry, null, null, BehaviorList.HoldCover) },
        // 6 the breather: two engineers at the console and a guard by the shutter, unaware, facing their work
        new[] { new Post(0.6f, 0.82f, Face.Back, null, Calm), new Post(0.7f, 0.82f, Face.Back, null, Calm), new Post(0.88f, 0.5f, Face.Exit, null, Calm) },
        // 7 the gauntlet: spread down the corridor between the cones, the riot guard blocking the middle
        new[] { new Post(0.5f, 0.3f, Face.Entry), new Post(0.66f, 0.7f, Face.Entry, null, null, BehaviorList.Runner), new Post(0.82f, 0.35f, Face.Entry),
                new Post(0.94f, 0.65f, Face.Entry, "M_SwordNexus", null, BehaviorList.Hunter), new Post(0.6f, 0.5f, Face.Entry, null, null, BehaviorList.Defensive) },
        // 8 close quarters: the riot guard steps up first, two hunters behind the lockers come at you
        new[] { new Post(0.5f, 0.5f, Face.Entry, null, null, BehaviorList.Defensive), new Post(0.72f, 0.75f, Face.Entry, "M_SwordNexus", null, BehaviorList.Hunter), new Post(0.8f, 0.35f, Face.Entry, null, null, BehaviorList.Hunter) },
        // 9 false calm: on break at the tables, unaware, facing the counter
        new[] { new Post(0.55f, 0.55f, Face.Back, null, Calm), new Post(0.68f, 0.4f, Face.Exit, null, Calm), new Post(0.8f, 0.6f, Face.Back, null, Calm), new Post(0.9f, 0.75f, Face.Back, null, Calm) },
        // 10 the last stand: soldats hold the far desks under the monitor wall, the riot guard screens them
        new[] { new Post(0.82f, 0.6f, Face.Entry, null, null, BehaviorList.HoldCover), new Post(0.9f, 0.3f, Face.Entry, null, null, BehaviorList.HoldCover),
                new Post(0.6f, 0.45f, Face.Entry, null, null, BehaviorList.Defensive), new Post(0.72f, 0.75f, Face.Back) },
    };

    static void Stage(GameObject go, Marker_Character m, Frame f, Bounds fb, int index, int n)
    {
        if (index >= Posts.Length || n >= Posts[index].Length) return;
        var p = FilmTotals && FilmTotalPosts[index] != null ? (n < FilmTotalPosts[index].Length ? FilmTotalPosts[index][n] : Posts[index][n]) : Posts[index][n];
        float u = Mathf.Max(0.45f, p.u);
        // zw: the two engineers stand at the console itself, so the far pylon and the sloped blocks keep clear of them
        if (FilmRoom6Structures && index == 5 && n < 2) u = FilmRoom6Ramp ? (n == 0 ? 0.45f : 0.51f) : n == 0 ? 0.47f : 0.54f;
        // zz: the sword agent stands left of the steps, between them and the free cabinet
        if (FilmRoom8Structures && index == 7 && n == 1) u = 0.62f;
        go.transform.position = new Vector3(Mathf.Lerp(f.x0 + 1f, f.x1 - 1f, u), go.transform.position.y, Mathf.Lerp(fb.min.z + 2f, f.zb - 1.5f, p.v));
        var dir = p.face == Face.Entry ? Vector3.left : p.face == Face.Exit ? Vector3.right : p.face == Face.Back ? Vector3.forward : Vector3.back;
        go.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        if (p.alert.HasValue) m.Alertness = p.alert.Value;
        if (p.held != null) { m.ClearDefaultWeapons = true; m.Held_R = p.held; m.Held_L = null; }
        if (p.add != null && p.add.Length > 0) m.AddBehaviors = p.add;
        Debug.Log("NexusProminence: NX_POST " + index + "." + n + " " + m.Character + " u " + u.ToString("0.00") + " v " + p.v.ToString("0.00") + " face " + p.face
            + " alert " + m.Alertness + " held " + (m.Held_R ?? "-") + " add " + string.Join("+", (p.add ?? new BehaviorList[0]).Select(b => b.ToString()).ToArray()));
    }

    // ---- Round 4 (build zn): each room its own place and its own beat (concern 1, "boring and incoherent").
    // From the film frames (film-match-zm_film) and ludowright's one idea per room: the light level rises and
    // falls along the raid (calm entry, red alarm, bright office, floodlit siege, dark loft, breather, dark
    // gauntlet, red close quarters, false calm, bright finale), each room gets one landmark the film supports,
    // and the door the player comes in by carries a lit header (way in, left) and the way on a red one (right).
    // Landmarks are thin wall or high pieces (no colliders; Solidify skips them), so cover and fights stay as
    // they were. RoomIdentity false = the zm rooms. LighterWalls: the film's walls are mid gray, ours were
    // charcoal and hid the round 3 panels; false = the zm values (tiles *_light come from make_textures.py).
    const bool RoomIdentity = true;
    const bool LighterWalls = true;
    // Round 8 (build zr): the way-in (lit) and way-on (red) headers were boxes 2.2 m long across the room, so on a
    // side-wall door they stuck out into the room as a short stick, and near the camera side they were hidden; only
    // rooms 1, 3 and 8 showed one. WallHeaders lays both flat along the wall above the chained door, the same in
    // every room. false = the zn placement.
    const bool WallHeaders = false; // judge r8: zq headers won appeal in rooms 1, 5, 8 (the flat bar hid the red way-on bar); kept off
    // ambient r,g,b and lamp colour r,g,b per room; a negative lamp red keeps the room's own lamp colour
    static readonly float[,] IdentityLight = {
        { 0.30f, 0.29f, 0.29f, 1f, 0.90f, 0.88f },   // 1 street: calm entry, neutral gray under the red sky (judge r4)
        { 0.55f, 0.16f, 0.16f, -1f, 0f, 0f },        // 2 wanted hall: the alarm (its own deep red grade)
        { 0.38f, 0.35f, 0.35f, 1f, 0.92f, 0.90f },   // 3 office: plain white office light after the red hall
        { 0.42f, 0.36f, 0.36f, 1f, 0.85f, 0.80f },   // 4 cargo bay: floodlit siege
        { 0.22f, 0.21f, 0.21f, 1f, 0.85f, 0.83f },   // 5 sniper loft: dim gray below, light on the ledge (judge r4)
        { 0.33f, 0.30f, 0.30f, 1f, 0.88f, 0.86f },   // 6 garage: the breather
        { 0.17f, 0.13f, 0.14f, -1f, 0f, 0f },        // 7 corridor: dark gauntlet, only the spot cones
        { 0.30f, 0.27f, 0.27f, 1f, 0.60f, 0.58f },   // 8 locker room: gray, lamps warm red from above (film 8:15; judge r4)
        { 0.34f, 0.29f, 0.28f, 1f, 0.78f, 0.70f },   // 9 break room: false calm
        { 0.48f, 0.24f, 0.24f, 1f, 0.50f, 0.48f } }; // 10 tower: bright red finale

    // Round 5 (build zo): rooms 3, 5 and 8 still read as one dark box. The film frames are warmer and lighter
    // there (median film vs map: office 114,40,43 vs 48,34,35; loft 61,39,36 vs 54,38,38; lockers 85,41,40 vs
    // 39,24,25): a mauve office under white light, a warm brown loft with a red skirting line, a locker room
    // lit red from above (a red floor pool was tried and read as a rug; dropped). FilmGrade false = the zn light.
    const bool FilmGrade = true;
    static readonly Dictionary<int, float[]> FilmGradeLight = new Dictionary<int, float[]> {
        { 2, new[] { 0.52f, 0.40f, 0.42f, 1f, 0.90f, 0.90f } },   // 3 office: mauve walls, white light
        { 4, new[] { 0.32f, 0.25f, 0.23f, 1f, 0.80f, 0.70f } },   // 5 loft: warm brown, ledge lit
        { 7, new[] { 0.42f, 0.25f, 0.25f, 1f, 0.45f, 0.42f } } }; // 8 lockers: red light from above

    // Round 8 (build zr), whole-map coherence: walking the run, the corridor (room 7) was the seam that broke it,
    // luma 22 against 40 and 31 either side and the film's 44 (film median 62,31,30, map 18,12,13), and the garage
    // (room 6) was neutral gray where the film is red (116,34,32 vs 54,42,42). FilmSeams grades both from the film
    // and keeps the corridor the darkest room of the run. false = the zq light.
    const bool FilmSeams = true;
    static readonly Dictionary<int, float[]> FilmSeamLight = new Dictionary<int, float[]> {
        { 5, new[] { 0.44f, 0.27f, 0.26f, 1f, 0.80f, 0.76f } },   // 6 garage: red-brown, as the film at 6:25
        { 6, new[] { 0.30f, 0.17f, 0.17f, -1f, 0f, 0f } } };      // 7 corridor: dark red, still the darkest; cones unchanged

    static void IdentityLights(RoomObj room, int index)
    {
        if (!RoomIdentity || index >= IdentityLight.GetLength(0)) return;
        // a copy: round 5 wrote the film grade back into the shared table (harmless once, wrong on a second Build)
        var g = new float[6]; for (int k = 0; k < 6; k++) g[k] = IdentityLight[index, k];
        if (FilmGrade && FilmGradeLight.ContainsKey(index)) g = FilmGradeLight[index];
        if (FilmSeams && FilmSeamLight.ContainsKey(index)) g = FilmSeamLight[index];
        room.AmbientColor = new Color(g[0], g[1], g[2]);
        if (g[3] >= 0f)
            foreach (var l in room.GetComponentsInChildren<Light>(true)) l.color = new Color(g[3], g[4], g[5]);
        Debug.Log("NexusProminence: " + room.name + " identity ambient " + room.AmbientColor);
    }

    static void Header(Frame f, string name, Vector3 d, string mat)
    {
        if (d.z > f.zb - 1f) { Box(f, name, d.x, f.y + 5.0f, f.zb - 0.1f, 2.6f, 0.22f, 0.08f, mat); return; } // a back-wall door
        bool left = d.x < f.Cx;
        Box(f, name, left ? f.x0 + 0.1f : f.x1 - 0.1f, f.y + 5.0f, d.z, 0.08f, 0.22f, 2.6f, mat);
    }

    static void Identity(Frame f, RoomObj room, int index)
    {
        if (!RoomIdentity) return;
        if (f.Doors.Count > 0)
        {
            var din = f.Doors.OrderBy(d => d.x).First(); var dout = f.Doors.OrderBy(d => d.x).Last();
            if (WallHeaders)
            {
                // round 8: the same sign in every room, flat on the side wall above the hazard bar of the chained door
                // (ChainDoors links the leftmost and rightmost door, the same picks as here)
                Header(f, "EntryHeader", din, "lit");
                if (dout != din) Header(f, "ExitHeader", dout, "screen");
            }
            else
            {
                // owner 2026-11-14 (build zu): on a side-wall door this box stuck out of the wall and hung in front of
                // the doorway as a floating red dash; only back-wall doors keep it, flat on the wall
                if (din.z > f.zb - 1f) Box(f, "EntryHeader", din.x, f.y + 4.9f, f.zb - 0.12f, 2.2f, 0.18f, 0.12f, "lit");
                if (dout != din && dout.z > f.zb - 1f) Box(f, "ExitHeader", dout.x, f.y + 4.9f, f.zb - 0.12f, 2.2f, 0.18f, 0.12f, "screen");
            }
        }
        switch (index)
        {
            case 0: // checkpoint boom barrier folded against the back wall, red and white
                if (FilmRoom1Structures) break; // zzb: the film's sill runs where the boom stood, and the film has no boom
                Box(f, "BoomPost", f.x0 + 2.2f, f.y + 0.7f, f.zb - 0.5f, 0.3f, 1.4f, 0.3f, "dark");
                for (int k = 0; k < 6; k++) Box(f, "BoomArm", f.x0 + 2.6f + k * 0.5f, f.y + 1.2f, f.zb - 0.5f, 0.5f, 0.16f, 0.12f, k % 2 == 0 ? "red" : "white");
                break;
            case 1: // alarm beacons high on the back wall
                foreach (var t0 in new[] { 0.3f, 0.7f }) { float t = t0; for (int g = 0; g < 12 && f.Doors.Any(d => Mathf.Abs(d.x - (f.x0 + t * f.W)) < 2.6f); g++) t += t0 < 0.5f ? -0.03f : 0.03f; Box(f, "AlarmBase", f.x0 + t * f.W, f.y + 5.7f, f.zb - 0.15f, 0.5f, 0.2f, 0.3f, "dark"); Cyl(f, "AlarmBeacon", f.x0 + t * f.W, f.y + 6.0f, f.zb - 0.15f, 0.35f, 0.4f, "screen"); }
                break;
            case 3: // hazard posts either side of the loading door
                foreach (var dx in new[] { -1.8f, 1.8f }) for (int k = 0; k < 6; k++) Box(f, "HazardPost", f.x1 - 3f + dx, f.y + 0.4f + k * 0.8f, f.zb - 0.1f, 0.4f, 0.8f, 0.06f, k % 2 == 0 ? "red" : "dark");
                break;
            case 2: // office: a light mauve wall dado under the red window, as the film's office wall
                if (FilmGrade) Box(f, "OfficeDado", f.Cx + 3f, f.y + 0.69f, f.zb - 0.08f, 12.4f, 1.38f, 0.04f, "panel"); // stops under the window frame
                break;
            case 4: // light strip under the ledge: the high ground is the bright place
                if (FilmGrade) Box(f, "Skirting", (f.x0 + 0.2f + f.x1 - 5.2f) / 2, f.y + 0.15f, f.zb - 0.08f, f.x1 - 5.4f - f.x0, 0.3f, 0.05f, "red"); // film 8:45 red skirting

                if (FilmRoom5Structures) Box(f, "LedgeGlow", f.x0 + 0.75f * f.W, f.y + LoftDeck - LoftSlab - 0.05f, f.zb - 0.4f, 0.26f * f.W - 0.6f, 0.08f, 0.5f, "lit"); // under the film slab (zx)
                else Box(f, "LedgeGlow", (f.Cx - 2f + f.x1 - 1f) / 2, f.y + 3.2f, f.zb - 1.0f, f.x1 - 1f - (f.Cx - 2f) - 0.4f, 0.08f, 0.6f, "lit");
                break;
            case 5: // parking bay lines on the garage floor
                for (int k = 0; k < 4; k++) Box(f, "BayLine", f.x0 + 4f + k * 4f, f.y + 0.012f, f.zb - 3f, 0.15f, 0.02f, 3.5f, "white");
                break;
            case 8: // a lit arrow on the back wall pointing the way on (the film's break room has an arrow sign)
                Box(f, "ArrowShaft", f.x0 + 9f, f.y + 3.3f, f.zb - 0.1f, 1.6f, 0.25f, 0.06f, "screen");
                Box(f, "ArrowHeadA", f.x0 + 9.85f, f.y + 3.47f, f.zb - 0.1f, 0.6f, 0.22f, 0.06f, "screen").transform.rotation = Quaternion.Euler(0, 0, -40f);
                Box(f, "ArrowHeadB", f.x0 + 9.85f, f.y + 3.13f, f.zb - 0.105f, 0.6f, 0.22f, 0.06f, "screen").transform.rotation = Quaternion.Euler(0, 0, 40f);
                break;
        }
    }

    // Every visible lamp must light (Workshop feedback: "lights that dont light"). The game allows 4 lights
    // a room: the room's own lights move under its lamps (a spot pointing down, inside the lamp's Intact
    // group, so shooting the lamp also puts its light out); missing ones are added up to the cap.
    const int LightCap = 4;
    static void LightTheLamps(RoomObj room, Frame f)
    {
        var lamps = f.Parent.GetComponentsInChildren<Transform>(true).Where(t => t.name.StartsWith("CO_NX_Lamp")).Take(LightCap).ToList();
        if (lamps.Count == 0) return;
        var lights = room.GetComponentsInChildren<Light>(true).ToList();
        var template = lights.FirstOrDefault();
        for (int i = 0; i < lamps.Count; i++)
        {
            Light l;
            if (i < lights.Count) l = lights[i];
            else if (lights.Count < LightCap) { l = new GameObject("NX_LampLight").AddComponent<Light>(); if (template != null) { l.intensity = template.intensity; l.shadows = template.shadows; } lights.Add(l); }
            else break;
            var intact = lamps[i].Find("Intact") ?? lamps[i];
            l.transform.SetParent(intact, true);
            l.transform.position = lamps[i].position + Vector3.down * 0.3f;
            l.transform.rotation = Quaternion.Euler(90f, 0, 0);
            l.type = LightType.Spot; l.spotAngle = 70f; l.range = 9f;
            l.color = new Color(1f, 0.45f, 0.42f);
            l.gameObject.isStatic = false;
        }
        Debug.Log("NexusProminence: " + room.name + " " + lamps.Count + " lamps lit");
    }

    // Round 9 (build zs): the last of the Playground sample in sight was its wall mesh (room_pg_room2_walls),
    // the same mouldings in every room. MapWalls keeps that mesh's collider exactly as it is (room sizes,
    // doors, camera, cover and AI unchanged), switches its renderer off (the game never re-enables room
    // MeshRenderers: decompiled Room_Main.MakeChildrenVisible touches lights, terrain and backgrounds only)
    // and draws the map's own panels where the collider is solid: rays every Step metres along the back and
    // side walls find the solid cells and the door openings; cells merge into boxes in NX_Wall (tiled).
    // false = the sample's wall mesh again.
    const bool MapWalls = true;
    const float MapWallStep = 0.2f, MapWallThick = 0.3f;
    static void BuildMapWalls(Frame f, RoomObj room)
    {
        if (!MapWalls) return;
        var mfs = room.GetComponentsInChildren<MeshFilter>(true).Where(m => m.name.StartsWith("room_pg_room2_walls")).ToList();
        if (mfs.Count == 0) { Debug.LogWarning("NexusProminence: " + room.name + " has no sample wall mesh; MapWalls skipped"); return; }
        Physics.SyncTransforms();
        var cols = mfs.SelectMany(m => m.GetComponents<Collider>()).ToArray();
        if (cols.Length == 0) { Debug.LogWarning("NexusProminence: " + room.name + " sample walls have no collider; MapWalls skipped"); return; }
        float top = mfs.Max(m => m.GetComponent<Renderer>() != null ? m.GetComponent<Renderer>().bounds.max.y : f.y + 6f);
        var parent = new GameObject("NX_MapWalls").transform;
        parent.SetParent(f.Parent.parent, false); parent.gameObject.isStatic = true;
        int boxes = 0;
        // one wall: walk u along it; at each u sample heights; a solid run of cells is a box column
        System.Action<string, float, float, System.Func<float, float, Ray>, System.Action<float, float, float, float>> wall = (label, u0, u1, ray, make) =>
        {
            int nu = Mathf.CeilToInt((u1 - u0) / MapWallStep), nv = Mathf.CeilToInt((top - f.y) / MapWallStep);
            var solid = new bool[nu, nv];
            for (int i = 0; i < nu; i++)
                for (int j = 0; j < nv; j++)
                {
                    var r = ray(u0 + (i + 0.5f) * MapWallStep, f.y + (j + 0.5f) * MapWallStep); RaycastHit h;
                    foreach (var c in cols) if (c.Raycast(r, out h, 3f)) { solid[i, j] = true; break; }
                }
            // vertical runs per column, then merge neighbouring columns with the same run
            var open = new Dictionary<long, int>(); // run key -> start column
            System.Func<int, List<long>> runs = i =>
            {
                var l = new List<long>(); if (i >= nu) return l;
                for (int j = 0; j < nv; j++) if (solid[i, j]) { int a = j; while (j + 1 < nv && solid[i, j + 1]) j++; l.Add(((long)a << 16) | (long)j); }
                return l;
            };
            for (int i = 0; i <= nu; i++)
            {
                var now = new HashSet<long>(runs(i));
                foreach (var k in open.Keys.ToList())
                    if (!now.Contains(k))
                    {
                        int a = (int)(k >> 16), b = (int)(k & 0xffff), s0 = open[k];
                        make(u0 + s0 * MapWallStep, u0 + i * MapWallStep, f.y + a * MapWallStep, f.y + (b + 1) * MapWallStep);
                        open.Remove(k); boxes++;
                    }
                foreach (var k in now) if (!open.ContainsKey(k)) open[k] = i;
            }
        };
        float t = MapWallThick, zf = f.zf;
        var src = mfs[0].GetComponent<MeshRenderer>();
        System.Action<string, Vector3, Vector3> cube = (n, c, sz) =>
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cube); Object.DestroyImmediate(g.GetComponent<Collider>());
            g.name = "NX_MapWall_" + n; g.transform.SetParent(parent, true); g.transform.position = c; g.transform.localScale = sz;
            var mr = g.GetComponent<MeshRenderer>(); mr.sharedMaterial = f.M["wall"]; g.isStatic = true;
            // judge r9: panels casting shadows threw a dark wedge the sample mesh never did; copy its shadow settings
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; if (src != null) mr.receiveShadows = src.receiveShadows; // the sample mesh is inward-facing only, so it never shadowed the room
        };
        // back wall: x along, rays toward +z from just inside the inner face
        wall("back", f.x0, f.x1, (u, y) => new Ray(new Vector3(u, y, f.zb - 0.5f), Vector3.forward),
            (a, b, y0, y1) => cube("Back", new Vector3((a + b) / 2, (y0 + y1) / 2, f.zb + t / 2), new Vector3(b - a, y1 - y0, t)));
        wall("left", zf, f.zb, (u, y) => new Ray(new Vector3(f.x0 + 0.5f, y, u), Vector3.left),
            (a, b, y0, y1) => cube("Left", new Vector3(f.x0 - t / 2, (y0 + y1) / 2, (a + b) / 2), new Vector3(t, y1 - y0, b - a)));
        wall("right", zf, f.zb, (u, y) => new Ray(new Vector3(f.x1 - 0.5f, y, u), Vector3.right),
            (a, b, y0, y1) => cube("Right", new Vector3(f.x1 + t / 2, (y0 + y1) / 2, (a + b) / 2), new Vector3(t, y1 - y0, b - a)));
        foreach (var m in mfs) { var mr = m.GetComponent<MeshRenderer>(); if (mr != null) mr.enabled = false; }
        Debug.Log("NexusProminence: " + room.name + " map walls " + boxes + " panels (sample wall renderer off, collider kept), top " + top.ToString("0.0"));
    }

    // The sample room's camera-side wall is six uneven triangulated planes (Workshop feedback). Swap its
    // mesh for one plain box of the same bounds; the object, its layer and components stay as they were.
    static void BoxFourthWall(RoomObj room)
    {
        foreach (var mf in room.GetComponentsInChildren<MeshFilter>(true).Where(m => m.name.StartsWith("room_pg_room2_4thwall")))
        {
            if (mf.sharedMesh == null) continue;
            var b = mf.sharedMesh.bounds;
            Debug.Log("NexusProminence: " + room.name + " 4th wall bounds " + b.center + " " + b.size);
            // a thin slab on its thinnest axis (a full box of the bounds sat in front of the camera and
            // could fill the floor); the camera-side wall is never drawn, so its renderer goes off
            var sz = b.size; int ax = sz.x < sz.y ? (sz.x < sz.z ? 0 : 2) : (sz.y < sz.z ? 1 : 2);
            float th = Mathf.Clamp(sz[ax], 0.2f, 0.3f); var c = b.center; c[ax] = b.min[ax] + sz[ax] / 2; sz[ax] = th;
            b = new Bounds(c, sz);
            var mr = mf.GetComponent<MeshRenderer>(); if (mr != null) mr.enabled = false;
            string path = MatDir + "/NX_FourthWallBox.asset";
            var box = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            bool fresh = box == null;
            if (fresh) box = new Mesh { name = "NX_FourthWallBox" };
            var tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cube = tmp.GetComponent<MeshFilter>().sharedMesh;
            box.Clear();
            box.vertices = cube.vertices.Select(v => b.center + Vector3.Scale(v, b.size)).ToArray();
            box.normals = cube.normals; box.uv = cube.uv; box.triangles = cube.triangles;
            box.RecalculateBounds();
            Object.DestroyImmediate(tmp);
            if (fresh) AssetDatabase.CreateAsset(box, path); else EditorUtility.SetDirty(box);
            mf.sharedMesh = box;
            var mc = mf.GetComponent<MeshCollider>(); if (mc != null) mc.sharedMesh = box;
        }
    }

    static float WallHit(Collider[] walls, Vector3 origin, Vector3 dir, float fallback)
    {
        float best = float.MaxValue; RaycastHit hit;
        foreach (var c in walls)
            if (c.Raycast(new Ray(origin, dir), out hit, 200f) && hit.distance < best) best = hit.distance;
        if (best == float.MaxValue) return fallback;
        var p = origin + dir * best;
        return Vector3.Dot(p, dir) * (dir.x + dir.y + dir.z) ; // the hit's coordinate on that axis
    }

    // ---- shared shell: bands, skirting, seams, panels, ceiling pipe, door frames ----
    // look proposal 2 (2026-11-11): the rooms ran together with the same band everywhere, so each room gets
    // its own band height, thickness and tone (red or maroon), and rooms with their own centrepiece drop the
    // side-wall banners. Set VaryBands false to go back to one band.
    const bool VaryBands = true;
    static readonly float[,] BandByRoom = { // bandLo, bandH, maroon (1) or red (0)
        { 1.05f, 0.55f, 0 }, { 0.75f, 0.35f, 0 }, { 1.4f, 0.4f, 1 }, { 1.05f, 0.9f, 1 }, { 1.7f, 0.3f, 0 },
        { 0.6f, 0.7f, 0 }, { 1.2f, 0.25f, 1 }, { 1.05f, 0.55f, 1 }, { 0.9f, 0.45f, 0 }, { 1.5f, 0.6f, 0 } };
    static readonly int[] OwnCentrepiece = { 0, 1, 6, 9 }; // street window, wanted hall, lamp corridor, tower

    static void Shell(Frame f, RoomObj room, int index)
    {
        float bandLo = 1.05f, bandH = 0.55f; string bandMat = "red";
        if (VaryBands && index < BandByRoom.GetLength(0))
        { bandLo = BandByRoom[index, 0]; bandH = BandByRoom[index, 1]; if (BandByRoom[index, 2] > 0) bandMat = "maroon"; }
        // back wall
        Box(f, "Band_Back", f.Cx, f.y + bandLo + bandH / 2, f.zb - 0.06f, f.W, bandH, 0.12f, bandMat);
        Box(f, "Skirt_Back", f.Cx, f.y + 0.14f, f.zb - 0.06f, f.W, 0.28f, 0.12f, "red");
        Box(f, "Seam_Back", f.Cx, f.y + 0.01f, f.zb - 0.35f, f.W, 0.02f, 0.18f, "red");
        // side walls
        foreach (var side in new[] { -1, 1 })
        {
            float x = side < 0 ? f.x0 + 0.06f : f.x1 - 0.06f;
            string s = side < 0 ? "L" : "R";
            // band and skirting stop at each side door (they ran across the doorways)
            var gaps = f.Doors.Where(d => (d.x < f.Cx) == (side < 0) && Mathf.Abs(d.x - x) < 3f).Select(d => d.z).OrderBy(z => z).ToList();
            float z0 = f.zf;
            foreach (var gz in gaps.Concat(new[] { f.zb + 1.5f }))
            {
                float z1 = Mathf.Min(gz - 1.5f, f.zb);
                if (z1 - z0 > 0.2f)
                {
                    Box(f, "Band_" + s, x, f.y + bandLo + bandH / 2, (z0 + z1) / 2, 0.12f, bandH + 0.02f, z1 - z0, bandMat); // zzh: 0.01 past the back band at both edges (S26)
                    Box(f, "Skirt_" + s, x, f.y + 0.145f, (z0 + z1) / 2, 0.12f, 0.29f, z1 - z0, "red");
                }
                z0 = gz + 1.5f;
            }
            Box(f, "Seam_" + s, x + side * -0.35f, f.y + 0.013f, f.Cz, 0.18f, 0.02f, f.D, "red");
        }
        // back wall panels (pilasters) and the pipe along the top, as in the corridor shots
        for (float x = f.x0 + 4f; x < f.x1 - 2f; x += 7.5f)
            Box(f, "Pilaster", x, f.y + 3.2f, f.zb - 0.12f, 0.45f, 6.4f, 0.2f, "panel");
        Box(f, "Pipe_Back", f.Cx, f.y + 6.0f, f.zb - 0.3f, f.W, 0.14f, 0.14f, "steel");
        for (float x = f.x0 + 2f; x < f.x1; x += 6f)
            Box(f, "PipeClamp", x, f.y + 6.1f, f.zb - 0.2f, 0.1f, 0.35f, 0.2f, "dark");
        var doors = room.GetComponentsInChildren<ProxyObj_Door>(true);
        foreach (var side in new[] { -1, 1 })
        {
            float x = side < 0 ? f.x0 + 0.07f : f.x1 - 0.07f;
            var sideDoors = doors.Where(d => (d.transform.position.x < f.Cx) == (side < 0)).Select(d => d.transform.position.z).ToArray();
            // the video's diagonal red stripe climbing each side wall from the band toward the ceiling at the back
            var diag = Box(f, "DiagStripe", x, f.y + 3.6f, f.zb - Mathf.Min(3.2f, f.D * 0.25f), 0.12f, 0.5f, 6.5f, "red");
            diag.transform.rotation = Quaternion.Euler(-38f, 0, 0);
            // a banner on each side wall, as far from that wall's doors as possible
            float bz = new[] { f.zb - 3f, f.Cz, f.zf + 4f }
                .OrderByDescending(z => sideDoors.Length == 0 ? 99f : sideDoors.Min(dz => Mathf.Abs(dz - z))).First();
            bool ownPiece = VaryBands && System.Array.IndexOf(OwnCentrepiece, index) >= 0;
            if (!ownPiece && (sideDoors.Length == 0 || sideDoors.Min(dz => Mathf.Abs(dz - bz)) > 2.2f)) Banner(f, x, f.y + 3.4f, bz, true);
        }
        // dark corner posts where the back wall meets the sides, and floor panel seams
        Box(f, "CornerPost", f.x0 + 0.3f, f.y + 3.6f, f.zb - 0.3f, 0.6f, 7.2f, 0.6f, "dark");
        Box(f, "CornerPost", f.x1 - 0.3f, f.y + 3.6f, f.zb - 0.3f, 0.6f, 7.2f, 0.6f, "dark");
        for (float z = f.zb - 4f; z > f.zf + 1f; z -= 4f)
            Box(f, "FloorSeam", f.Cx, f.y + 0.006f, z, f.W - 1f, 0.004f, 0.05f, "dark");
        // the open floor: flat details only (nothing to walk around): drain grates, a painted red line
        // across the front third, loose papers; the game's cover props and the enemies fill the rest
        var rng = new System.Random(room.name.GetHashCode());
        Box(f, "FloorLine", f.Cx, f.y + 0.008f, f.zf + f.D * 0.3f, f.W - 3f, 0.004f, 0.12f, "red");
        for (int i = 0; i < Mathf.Max(2, (int)(f.W / 12f)); i++)
        {
            float gx = Mathf.Lerp(f.x0 + 3f, f.x1 - 3f, (i + 0.5f) / Mathf.Max(2, (int)(f.W / 12f))), gz = f.zf + f.D * (0.15f + 0.5f * (float)rng.NextDouble());
            Box(f, "Grate", gx, f.y + 0.01f, gz, 1.4f, 0.008f, 1.0f, "dark");
            for (int k = 0; k < 4; k++) Box(f, "GrateSlot", gx - 0.45f + k * 0.3f, f.y + 0.016f, gz, 0.1f, 0.004f, 0.8f, "black");
        }
        for (int i = 0; i < (int)(f.W / 4f); i++)
        {
            var paper = Box(f, "Paper", f.x0 + 2f + (float)rng.NextDouble() * (f.W - 4f), PaperY(f), f.zf + 1f + (float)rng.NextDouble() * (f.D - 3f), 0.5f, 0.004f, 0.65f, "white");
            paper.transform.rotation = Quaternion.Euler(0, (float)rng.NextDouble() * 180f, 0);
        }
        // door frames: black opening, dark jambs, red-and-dark hazard bar above
        foreach (var door in doors)
        {
            var p = door.transform.position;
            bool left = p.x < f.Cx;
            float x = left ? f.x0 + 0.04f : f.x1 - 0.04f;
            float inward = left ? 1 : -1;
            Box(f, "DoorHole", x, f.y + 1.9f, p.z, 0.05f, 3.8f, 2.6f, "black");
            Box(f, "DoorJambA", x + inward * 0.1f, f.y + 2.0f, p.z - 1.4f, 0.2f, 4.0f, 0.22f, "dark");
            Box(f, "DoorJambB", x + inward * 0.1f, f.y + 2.0f, p.z + 1.4f, 0.2f, 4.0f, 0.22f, "dark");
            Box(f, "DoorHeader", x + inward * 0.1f, f.y + 4.05f, p.z, 0.24f, 0.22f, 3.0f, "dark");
            for (int k = 0; k < 5; k++)
                Box(f, "Hazard", x + inward * 0.08f, f.y + 4.55f, p.z - 1.2f + k * 0.6f, 0.08f, 0.4f, 0.3f, k % 2 == 0 ? "red" : "dark");
        }
    }

    // ---- per-room dressing (layout and colour from the frames; no art, logos or signs copied) ----

    // 1: street checkpoint (video 0:19): a small room, the back wall one wide window over the red city
    // (towers, billboards, a hung broken sign), a counter running along the left side wall with a red glow,
    // a door with an emblem on the right side wall, trash bin and cups in the back right corner.
    static void Street(Frame f)
    {
        var rng = new System.Random(7);
        Box(f, "Asphalt", f.Cx, f.y + 0.004f, f.Cz, f.W, 0.008f, f.D, "asphalt");
        for (int i = 0; i < 8; i++)
            Box(f, "Stain", f.x0 + 2f + (float)rng.NextDouble() * (f.W - 4f), f.y + 0.01f, f.zf + 1f + (float)rng.NextDouble() * (f.D - 2f),
                0.6f + (float)rng.NextDouble() * 1.8f, 0.006f, 0.4f + (float)rng.NextDouble() * 1.2f, "stain");

        // back: one window almost wall to wall
        float wx0 = FilmRoom1Structures ? f.x0 + 0.09f * f.W : f.x0 + 1.4f, wx1 = FilmRoom1Structures ? f.x0 + 0.79f * f.W : f.x1 - 1.4f, wy0 = f.y + 1.4f, wy1 = f.y + 6.8f, wz = f.zb - 0.05f;
        Box(f, "Window_Sky", (wx0 + wx1) / 2, (wy0 + wy1) / 2, wz, wx1 - wx0, wy1 - wy0, 0.04f, "sky");
        int towerN = 0;
        for (float x = wx0 + 0.2f; x < wx1 - 0.6f; )
        {
            float w = 1.0f + (float)rng.NextDouble() * 1.8f;
            float h = 2.4f + (float)rng.NextDouble() * 3.0f;
            float cx = Mathf.Min(x + w / 2, wx1 - w / 2);
            Box(f, "Tower", cx, wy0 + h / 2, wz - 0.06f - 0.004f * (towerN++ % 2), w, h, 0.04f, rng.NextDouble() < 0.5 ? "black" : "dark"); // zzh: front 0.01 before the band (owner's glitch shot 7)
            for (float yy = wy0 + 0.3f; yy < wy0 + h - 0.3f; yy += 0.4f)
                for (float xx = cx - w / 2 + 0.2f; xx < cx + w / 2 - 0.15f; xx += 0.3f)
                    if (rng.NextDouble() < 0.6) Box(f, "LitWin", xx, yy, wz - 0.08f, 0.14f, 0.2f, 0.02f, "lit");
            if (rng.NextDouble() < 0.45)
            {
                Box(f, "BillFrame", cx, wy0 + h * 0.6f, wz - 0.1f, w * 0.8f, 1.2f, 0.02f, "white");
                Box(f, "Bill", cx, wy0 + h * 0.6f, wz - 0.11f, w * 0.7f, 1.05f, 0.02f, "screen");
            }
            x += w + 0.1f;
        }
        Box(f, "Window_Head", (wx0 + wx1) / 2, wy1 + 0.3f, f.zb - 0.3f, wx1 - wx0 + 0.8f, 0.6f, 0.6f, "dark");
        Box(f, "Window_SideA", wx0 - 0.2f, (wy0 + wy1) / 2, f.zb - 0.3f, 0.5f, wy1 - wy0, 0.6f, "dark");
        Box(f, "Window_SideB", wx1 + 0.2f, (wy0 + wy1) / 2, f.zb - 0.3f, 0.5f, wy1 - wy0, 0.6f, "dark");
        if (FilmRoom1Structures) FilmCheckpoint(f, wx0, wx1, wy0);
        else
        {
            Box(f, "Window_Sill", (wx0 + wx1) / 2, wy0 + 0.1f, f.zb - 0.35f, wx1 - wx0, 0.2f, 0.6f, "dark");
            Box(f, "Window_Glow", (wx0 + wx1) / 2, wy0 - 0.5f, f.zb - 0.1f, wx1 - wx0, 1.0f, 0.04f, "screen");
        }
        // hung, cracked sign in the middle of the window
        float sx = FilmRoom1Structures ? f.x0 + 0.42f * f.W : (wx0 + wx1) / 2;
        // (pulled back to the window plane: Workshop feedback said the outdoors reached into the room)
        Box(f, "SignRodA", sx - 1.2f, wy1 + 0.2f, f.zb - 0.45f, 0.12f, 1.2f, 0.12f, "dark");
        Box(f, "SignRodB", sx + 1.2f, wy1 + 0.2f, f.zb - 0.45f, 0.12f, 1.2f, 0.12f, "dark");
        Box(f, "Sign", sx, wy1 - 0.8f, f.zb - 0.45f, 3.2f, 1.1f, 0.15f, "dark");
        Box(f, "SignFace", sx, wy1 - 0.8f, f.zb - 0.54f, 2.9f, 0.85f, 0.02f, "glass");
        Box(f, "SignWire", sx + 0.4f, wy1 - 2.0f, f.zb - 0.5f, 0.03f, 1.6f, 0.03f, "black");

        // left side as in the film (0:19): a plain charcoal panelled wall with only the red skirting;
        // drop the Shell's red band, diagonal stripe and banner on this wall
        for (int c = f.Parent.childCount - 1; c >= 0; c--)
        {
            var ch = f.Parent.GetChild(c);
            if (ch.position.x < f.x0 + 2.5f && (ch.name == "NX_Band_L" || ch.name == "NX_DiagStripe" || ch.name.StartsWith("NX_Banner") || ch.name.StartsWith("NX_Hazard")))
                Object.DestroyImmediate(ch.gameObject);
        }
        // a flat charcoal facing over the template wall's red-and-black trim, then the skirting again
        Box(f, "FacingL", f.x0 + 0.03f, f.y + 3.6f, f.Cz, 0.04f, 7.2f, f.D + 1f, "wall");
        Box(f, "SkirtL2", f.x0 + 0.07f, f.y + 0.155f, f.Cz, 0.06f, 0.31f, f.D + 1f, "red");
        for (float z = f.zb - 2.5f; z > f.zf + 0.5f; z -= 2.5f)
            Box(f, "PanelSeamL", f.x0 + 0.1f, f.y + 3.4f, z, 0.03f, 6.8f, 0.06f, "dark");
        Box(f, "PanelRailL", f.x0 + 0.1f, f.y + 0.9f, f.Cz, 0.04f, 0.06f, f.D, "dark");

        // right side: door with an emblem, trash bin and cups in the back corner
        float dz = f.zf + f.D * 0.5f;
        if (FakeDoors)
        {
            Box(f, "SideDoorFrame", f.x1 - 0.08f, f.y + 2.6f, dz, 0.1f, 5.2f, 3.0f, "dark");
            Box(f, "SideDoor", f.x1 - 0.14f, f.y + 2.5f, dz, 0.04f, 4.8f, 2.6f, "panel");
            Box(f, "SideDoorEmblem", f.x1 - 0.17f, f.y + 3.6f, dz, 0.03f, 1.0f, 1.0f, "red");
            Box(f, "SideDoorSill", f.x1 - 0.3f, f.y + 0.15f, dz, 0.5f, 0.3f, 3.0f, "red");
        }
        Cyl(f, "TrashBin", f.x1 - 1.4f, f.y + 0.75f, f.zb - 1.2f, 1.1f, 1.5f, "steel");
        Box(f, "TrashLid", f.x1 - 1.4f, f.y + 1.52f, f.zb - 1.2f, 1.2f, 0.08f, 1.2f, "dark");
        for (int i = 0; i < 3; i++) Cyl(f, "Cup", f.x1 - 2.4f + i * 0.28f, f.y + 0.13f, f.zb - 1.9f - (i % 2) * 0.2f, 0.18f, 0.26f, "white");
        for (float x = f.x0 + 2f; x < f.x1 - 1f; x += 3.2f)
            Box(f, "PanelSeam", x, f.y + 7.5f, f.zb - 0.02f, 0.06f, 0.8f, 0.02f, "dark");
    }

    // ---- Film geometry, room 1 (build zzb): the film's street-front bay, 0:17-0:41.5 (structure sheet
    // ai-docs/notes/film-geometry/room-01-street-checkpoint.md). 1 m = 1.5 units. The window narrows to the
    // film's u 0.09-0.79 with wall each side. Solid on the one floor: the window sill as a low wall (top at
    // 0.9 m, 0.4 m deep, red-lit face below the ledge; the room behind it is outside, so it changes looks,
    // not routes), the right pilaster (u 0.79-0.88, 0.5 m proud), the left wall block and counter
    // are left out (they would stand by the left-wall door). The hung sign moves to the film's
    // u 0.42 (Street). Set false to build zza's room 1 (window wall to wall, thin sill at the glass).
    const bool FilmRoom1Structures = true;
    static void FilmCheckpoint(Frame f, float wx0, float wx1, float wy0)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f, sh = 0.9f * m, sd = 0.4f * m, sz = f.zb - sd / 2 - 0.05f;
        // 2: sill: a dark ledge 0.2 m thick on a low wall with a red-lit face, the whole window width
        // the sill starts 0.5 in from the glass edge (3.5 units clear of the left-wall door) and stops short of the side frame
        float s0 = wx0 + 0.5f, s1 = wx1 - 0.1f;
        var sill = Box(f, "SillWall", (s0 + s1) / 2, f.y + (sh - 0.2f * m) / 2, sz, s1 - s0, sh - 0.2f * m, sd, "dark");
        // wider than Solidify's wall-facing limit, so it gets its own collider: shots stop at it as in the film
        if (sill.GetComponent<BoxCollider>() == null) { sill.AddComponent<BoxCollider>(); sill.layer = ObstacleLayer; solidCount++; }
        filmStructures.Add(sill);
        Box(f, "SillLedge", (s0 + s1) / 2, f.y + sh - 0.1f * m, sz - 0.05f, s1 - s0, 0.2f * m, sd + 0.1f, "black");
        Box(f, "SillGlow", (s0 + s1) / 2, f.y + (sh - 0.2f * m) / 2, f.zb - sd - 0.07f, s1 - s0 - 0.3f, sh - 0.2f * m - 0.3f, 0.02f, "screen");
        // the skyline glass starts at the ledge: fill the gap between ledge and the old window bottom with dark wall
        if (wy0 > f.y + sh) Box(f, "SillBack", (wx0 + wx1) / 2, (f.y + sh + wy0) / 2, f.zb - 0.06f, wx1 - wx0, wy0 - f.y - sh, 0.04f, "wall");
        // 7: right pilaster, floor to ceiling, the red skirting wrapping it
        float px0 = wx1 + 0.5f, px1 = u(0.88f), pd = 0.5f * m;
        filmStructures.Add(Box(f, "CornerPilaster", (px0 + px1) / 2, f.y + 4.5f, f.zb - pd / 2, px1 - px0, 9f, pd, "wall"));
        Box(f, "CornerPilasterSkirt", (px0 + px1) / 2, f.y + 0.2f * m, f.zb - pd / 2, px1 - px0 + 0.06f, 0.35f * m, pd + 0.06f, "red");
        // right of the pilaster to the side wall: plain wall over the old glass
        Box(f, "WallRight", (px1 + f.x1) / 2, f.y + 4.5f, f.zb - 0.04f, f.x1 - px1, 9f, 0.04f, "wall");
        // 6: the left wall block and its counter are left out: the left-wall door stands 3.3 units out from the
        // back wall and both would sit within the door gate's 3.5 units (zzb first pass)
    }

    // 2: red-lit hall (video 1:28-2:00): wanted posters in red frames hung on rods, a glass display case of
    // supplies, tall dark doorways, red conduits from the ceiling, floor hatches, stripes on the band.
    static void WantedHall(Frame f)
    {
        if (FilmRoom2Structures) { FilmWanted(f); return; }
        float[] doors = { f.x0 + 0.30f * f.W, f.x0 + 0.78f * f.W };
        foreach (var x in doors)
        {
            Box(f, "BackDoorFrame", x, f.y + 2.4f, f.zb - 0.15f, 3.0f, 4.8f, 0.1f, "dark");
            Box(f, "BackDoor", x, f.y + 2.3f, f.zb - 0.2f, 2.5f, 4.6f, 0.04f, "black");
            Box(f, "DoorLightbox", x, f.y + 5.15f, f.zb - 0.2f, 2.0f, 0.35f, 0.06f, "lit"); // bright accent (aw.py light: flat values)
        }
        for (int i = 0; i < 3; i++)
        {
            float x = f.x0 + 0.12f * f.W + i * 0.27f * f.W;
            if (i == 1) x += 0.02f * f.W;
            float py = f.y + 3.3f;
            Box(f, "PosterRod", x, py + 2.0f, f.zb - 0.3f, 0.08f, 2.4f, 0.08f, "red");
            Box(f, "PosterFrame", x, py, f.zb - 0.3f, 2.3f, 2.9f, 0.12f, "red");
            Box(f, "Poster", x, py + 0.15f, f.zb - 0.38f, 1.8f, 2.1f, 0.02f, "white");
            Box(f, "PosterHead", x, py + 0.35f, f.zb - 0.41f, 0.9f, 1.05f, 0.02f, "panel");
            Box(f, "PosterShades", x, py + 0.45f, f.zb - 0.43f, 0.75f, 0.16f, 0.02f, "black");
            Box(f, "PosterCaption", x, py - 0.7f, f.zb - 0.41f, 1.8f, 0.4f, 0.02f, "dark");
        }
        // display case: dark cabinet, glowing glass, shelves with boxes and a medkit
        float cx = f.x0 + 0.55f * f.W;
        Box(f, "Case", cx, f.y + 2.3f, f.zb - 0.8f, 2.6f, 4.6f, 1.2f, "dark");
        var glass = Box(f, "CaseGlass", cx, f.y + 2.8f, f.zb - 1.42f, 2.1f, 3.0f, 0.03f, "glass");
        // shot glass: a jagged rim left in the frame and shards on the floor in front of the case
        var shards = new List<GameObject>
        {
            Box(f, "CaseGlassRimTop", cx, f.y + 4.2f, f.zb - 1.42f, 2.1f, 0.25f, 0.03f, "glass"),
            Box(f, "CaseGlassRimLeft", cx - 0.9f, f.y + 3.4f, f.zb - 1.42f, 0.3f, 1.2f, 0.03f, "glass"),
        };
        for (int k = 0; k < 6; k++)
        {
            var s = Box(f, "Shard", cx - 1.0f + k * 0.4f, f.y + 0.02f, f.zb - 1.7f - (k % 3) * 0.25f, 0.3f - (k % 2) * 0.12f, 0.02f, 0.18f, "glass");
            s.transform.rotation = Quaternion.Euler(0, k * 37f, 0);
            shards.Add(s);
        }
        Breakable(f, "CaseGlass", new Vector3(cx, f.y + 2.8f, f.zb - 1.2f), new Vector3(2.1f, 3.0f, 0.4f),
            new List<GameObject> { glass }, shards, GlassHits, "", GlassBreakSound);
        for (int k = 0; k < 3; k++)
        {
            float sy = f.y + 1.6f + k * 1.0f;
            Box(f, "CaseShelf", cx, sy, f.zb - 1.45f, 2.1f, 0.06f, 0.04f, "steel");
            Box(f, "CaseItem", cx - 0.5f, sy + 0.25f, f.zb - 1.47f, 0.5f, 0.4f, 0.02f, k == 1 ? "white" : "panel");
            Box(f, "CaseItem", cx + 0.4f, sy + 0.2f, f.zb - 1.47f, 0.6f, 0.3f, 0.02f, "steel");
        }
        Box(f, "CaseKeypad", cx - 0.85f, f.y + 2.4f, f.zb - 1.43f, 0.3f, 0.5f, 0.02f, "steel");
        Box(f, "CaseSlot", cx, f.y + 0.7f, f.zb - 1.43f, 1.4f, 0.5f, 0.02f, "black");
        // conduits down from the ceiling and stripes on the band
        foreach (var x in new[] { f.x0 + 0.42f * f.W, f.x0 + 0.92f * f.W })
            Box(f, "Conduit", x, f.y + 4.6f, f.zb - 0.2f, 0.25f, 5.2f, 0.2f, "red");
        foreach (var x in new[] { f.x0 + 0.2f * f.W, f.x0 + 0.66f * f.W })
            for (int k = 0; k < 4; k++) Box(f, "BandStripe", x + k * 0.5f, f.y + 1.32f, f.zb - 0.13f, 0.22f, 0.5f, 0.02f, "dark");
        // floor hatches with grilles
        foreach (var hx in new[] { f.x0 + 0.25f * f.W, f.x0 + 0.62f * f.W })
        {
            float hz = f.zb - 3.2f;
            Box(f, "HatchFrame", hx, f.y + 0.012f, hz, 2.6f, 0.024f, 1.8f, "steel");
            Box(f, "Hatch", hx, f.y + 0.02f, hz, 2.2f, 0.024f, 1.4f, "black");
            for (int i = 0; i < 5; i++) Box(f, "HatchSlot", hx - 0.8f + i * 0.4f, f.y + 0.03f, hz, 0.14f, 0.02f, 1.1f, "dark");
        }
    }

    // ---- Film geometry, room 2 (build zzd): the film's wanted hall, set C, 1:21.5-2:13 (structure sheet
    // ai-docs/notes/film-geometry/room-02-wanted-hall.md). 1 m = 1.5 units; pieces at the film's across positions
    // (u) at film size. Solid on the one floor: the floor hatch (a pit drawn on the floor inside a 0.25 m rim, one
    // collider over the whole hatch: a low obstacle mid-floor, not a route), the jutting wall block with its red
    // edge (the one piece of cover the film uses) and the vending machine (keeps the display glass that breaks).
    // Backdrop: the wanted case with its feed line to the ceiling, the red-framed panel round it, the recessed bay
    // with its hazard sill. Removed under the toggle: three hung posters, two fake back doors with light bars, two
    // ceiling conduits, two flat floor hatches, the centre display case. Set false to build zzc's wanted hall.
    const bool FilmRoom2Structures = true;
    static void FilmWanted(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f, zb = f.zb;
        // 2: red-framed wall panel u 0.08-0.48, 2.9 m tall, strip 0.2 m (floor line, both ends, top)
        float p0 = u(0.08f), p1 = u(0.48f), ph = 2.9f * m, st = 0.2f * m;
        Box(f, "WallPanel", (p0 + p1) / 2, f.y + ph / 2, zb - 0.06f, p1 - p0, ph, 0.04f, "panel");
        Box(f, "PanelFrameFoot", (p0 + p1) / 2, f.y + st / 2, zb - 0.1f, p1 - p0, st, 0.06f, "red");
        Box(f, "PanelFrameTop", (p0 + p1) / 2, f.y + ph - st / 2, zb - 0.1f, p1 - p0, st, 0.06f, "red");
        foreach (var x in new[] { p0 + st / 2, p1 - st / 2 }) Box(f, "PanelFrameEnd", x, f.y + ph / 2, zb - 0.105f, st + 0.02f, ph + 0.02f, 0.07f, "red");
        // 1: the wanted case, 1.3 x 1.6 m, bottom 0.5 m up, 0.2 m proud, lit portrait and a name plate; feed line to the ceiling
        float cx = u(0.24f), cw = 1.3f * m, chh = 1.6f * m, cb = f.y + 0.5f * m, cd = 0.2f * m;
        Box(f, "WantedCase", cx, cb + chh / 2, zb - 0.1f - cd / 2, cw, chh, cd, "dark");
        Box(f, "WantedCaseGlow", cx, cb + chh / 2 + 0.2f, zb - 0.12f - cd, cw - 0.4f, chh - 0.9f, 0.02f, "screen");
        Box(f, "WantedHead", cx, cb + chh / 2 + 0.3f, zb - 0.14f - cd, 0.6f, 0.75f, 0.02f, "panel");
        Box(f, "WantedPlate", cx, cb + 0.3f, zb - 0.12f - cd, cw - 0.5f, 0.3f, 0.02f, "white");
        float lx = cx + cw / 2 - 0.15f, ltop = f.y + 9f;
        Box(f, "FeedLine", lx, (cb + chh + ltop) / 2, zb - 0.15f, 0.14f, ltop - cb - chh, 0.14f, "red");
        // 4: jutting wall block at the panel's right end, 0.3 m wide and proud, full height, red edge strip
        float bw = 0.3f * m, bx = p1 + bw / 2;
        filmStructures.Add(Box(f, "WallBlock", bx, f.y + 4.5f, zb - bw / 2, bw, 9f, bw, "wall"));
        Box(f, "WallBlockEdge", bx, f.y + 4.5f, zb - bw - 0.02f, bw, 9f, 0.04f, "red");
        // 5: recessed bay at u 0.57: dark panel in a red frame, hazard sill 0.45 m up, chamfered red header bar
        float yx = u(0.57f), yw = 1.6f * m, yh = 2.0f * m, ys = f.y + 0.45f * m;
        Box(f, "BayPanel", yx, ys + yh / 2, zb - 0.135f, yw, yh, 0.02f, "black"); // zzh: in front of the wall band (owner's glitch shot 6)
        foreach (var x in new[] { yx - yw / 2 - 0.1f, yx + yw / 2 + 0.1f }) Box(f, "BayFrame", x, ys + yh / 2, zb - 0.11f, 0.2f, yh + 0.2f, 0.1f, "red");
        for (int k = 0; k < 6; k++) Box(f, "BaySill", yx - yw / 2 + (k + 0.5f) * yw / 6, ys - 0.08f, zb - 0.12f, yw / 6, 0.16f, 0.16f, k % 2 == 0 ? "red" : "dark");
        var hdr = Box(f, "BayHeader", yx, ys + yh + 0.25f, zb - 0.15f, yw + 0.6f, 0.3f, 0.25f, "red");
        hdr.transform.rotation = Quaternion.Euler(-20f, 0f, 0f);
        // 6: vending machine at u 0.73, 1.6 x 2.4 x 0.7 m, angled red-trimmed housing; its glass still breaks (JuiceBreakables)
        float vx = u(0.73f), vw = 1.6f * m, vh = 2.4f * m, vd = 0.7f * m, vz = zb - vd / 2 - 0.05f;
        filmStructures.Add(Model2(Box(f, "Vending", vx, f.y + vh / 2, vz, vw, vh, vd, "dark"), "nx_vending"));
        foreach (var s in new[] { -1, 1 })
        {
            // angled housing: the trim leans out from the wall, its back kept off the wall seam
            var trim = Box(f, "VendingTrim", vx + s * (vw / 2 + 0.1f), f.y + vh / 2 + 0.03f, vz - 0.1f, 0.2f, vh, vd - 0.1f, "red");
            trim.transform.rotation = Quaternion.Euler(0f, s * 8f, 0f);
        }
        Box(f, "VendingTop", vx, f.y + vh + 0.1f, vz, vw + 0.4f, 0.2f, vd + 0.1f, "red");
        float gz = zb - vd - 0.08f;
        var glass = Box(f, "VendingGlass", vx - 0.2f, f.y + vh * 0.55f, gz + 0.005f, vw - 0.8f, vh * 0.6f, 0.03f, "glass");
        Box(f, "VendingKeypad", vx + vw / 2 - 0.3f, f.y + vh * 0.55f, gz, 0.3f, 0.5f, 0.02f, "steel");
        Box(f, "VendingItem", vx - 0.2f, f.y + vh * 0.55f, gz + 0.02f, 0.5f, 0.4f, 0.02f, "white"); // the first aid kit
        var shards = new List<GameObject> { Box(f, "VendingGlassRim", vx - 0.2f, f.y + vh * 0.82f, gz, vw - 0.82f, 0.2f, 0.03f, "glass") };
        for (int k = 0; k < 5; k++)
        {
            var s = Box(f, "Shard", vx - 0.9f + k * 0.45f, f.y + 0.02f, gz - 0.3f - (k % 3) * 0.25f, 0.3f - (k % 2) * 0.12f, 0.02f, 0.18f, "glass");
            s.transform.rotation = Quaternion.Euler(0, k * 37f, 0);
            shards.Add(s);
        }
        Breakable(f, "VendingGlass", new Vector3(vx - 0.2f, f.y + vh * 0.55f, gz + 0.2f), new Vector3(vw - 0.8f, vh * 0.6f, 0.4f),
            new List<GameObject> { glass }, shards, GlassHits, "", GlassBreakSound);
        // 3: floor hatch at u 0.27, 2 m out: an open pit (drawn) inside a 0.25 m rim, slatted lid lying beside it
        float hx = u(0.27f), hz = zb - 2.0f * m, hw = 2.3f * m, hd = 1.2f * m, rh = 0.25f * m, rt = 0.15f * m;
        var hatch = Box(f, "HatchPit", hx, f.y + 0.012f, hz, hw - 2 * rt, 0.02f, hd - 2 * rt, "black");
        // under Solidify's 0.8 height floor: one collider over the whole hatch, the rim's height (a low obstacle; nobody walks into the pit)
        var hc = hatch.AddComponent<BoxCollider>();
        hc.size = new Vector3(hw / (hw - 2 * rt), rh / 0.02f, hd / (hd - 2 * rt)); hc.center = new Vector3(0f, (rh / 2 - 0.012f) / 0.02f, 0f);
        hatch.layer = ObstacleLayer; solidCount++;
        filmStructures.Add(hatch);
        foreach (var s in new[] { -1, 1 })
        {
            Box(f, "HatchRim", hx, f.y + rh / 2, hz + s * (hd - rt) / 2, hw, rh, rt, "steel");
            Box(f, "HatchRim", hx + s * (hw - rt) / 2, f.y + rh / 2, hz, rt, rh, hd - 2 * rt, "steel");
        }
        float lx0 = hx + hw / 2 + 0.3f + hw * 0.45f;
        Box(f, "HatchLid", lx0, f.y + 0.04f, hz, hw * 0.9f, 0.08f, hd * 0.9f, "dark");
        for (int k = 0; k < 5; k++) Box(f, "HatchLidSlat", lx0 - hw * 0.36f + k * hw * 0.18f, f.y + 0.09f, hz, 0.14f, 0.02f, hd * 0.8f, "steel");
        // 7, 8: the back doorway and the left-wall door are left out (fake doors stay off; doors unchanged)
    }

    // ---- Film geometry, room 3 (build zze): the film's controls office, 2:36-3:18 (structure sheet
    // ai-docs/notes/film-geometry/room-03-controls-office.md). 1 m = 1.5 units. Solid on the one floor: the counter
    // (u 0.12-0.48, 1.0 m tall, 0.6 m deep) standing 0.9 m clear of the back wall so defenders fit behind it, and
    // the low kerb along the back wall right of it (0.45 m, a solid plinth, not a step: prompt C1). Backdrop: the red
    // window over the counter, the pilaster at u 0.11, the wall block at u 0.50, the closed door under the
    // "controls" sign at u 0.60 (it never opens in the film), the notice and the tilted poster. The entry doorway at
    // the left stays BackDoorway (FakeDoors off: not built). Removed under the toggle: the right-of-centre window
    // and counter, filing cabinets, banner, clock, notices. Set false to build zzd's office.
    const bool FilmRoom3Structures = true;
    static void FilmOffice(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f, zb = f.zb;
        // 1: red window u 0.15-0.48, sill 1.35 m up, 1.8 m tall, set back in a dark frame
        float w0 = u(0.15f), w1 = u(0.48f), wy = f.y + 1.35f * m, wh = 1.8f * m;
        Box(f, "BackWindow", (w0 + w1) / 2, wy + wh / 2, zb - 0.04f, w1 - w0, wh, 0.03f, "screen");
        Box(f, "BackWindowFrame", (w0 + w1) / 2, wy - 0.1f, zb - 0.12f, w1 - w0, 0.2f, 0.16f, "dark");
        Box(f, "BackWindowFrame", (w0 + w1) / 2, wy + wh + 0.1f, zb - 0.12f, w1 - w0, 0.2f, 0.16f, "dark");
        // 2: counter, 1.0 m tall, 0.6 m deep, 0.9 m clear of the wall (a walkway behind it)
        float c0 = u(0.15f), c1 = u(0.48f), ch = 1.0f * m, cd = 0.6f * m, cz = zb - 0.9f * m - cd / 2;
        var counter = Model(Box(f, "Counter", (c0 + c1) / 2, f.y + ch / 2, cz, c1 - c0, ch, cd, "panel"), "nx_counter");
        if (counter.GetComponent<BoxCollider>() == null) { counter.AddComponent<BoxCollider>(); counter.layer = ObstacleLayer; solidCount++; }
        filmStructures.Add(counter);
        Box(f, "CounterTop", (c0 + c1) / 2, f.y + ch + 0.05f, cz, c1 - c0 + 0.3f, 0.1f, cd + 0.2f, "dark");
        // 4: pilaster at u 0.11, 0.9 m wide, 0.2 m proud, skirting wrapped; 8: wall block at u 0.50, 0.7 m wide
        foreach (var (a, w) in new[] { (0.11f, 0.9f), (0.50f, 0.7f) })
        {
            Box(f, "WallPier", u(a), f.y + 4.5f, zb - 0.15f, w * m, 9f, 0.3f, "wall");
            Box(f, "WallPierSkirt", u(a), f.y + 0.2f, zb - 0.15f, w * m + 0.06f, 0.4f, 0.36f, "red");
        }
        // 6: low kerb from the wall block to near the right corner, 0.45 m high, 0.9 m deep; solid plinth, its own collider
        float k0 = u(0.53f), k1 = u(KerbEnd), kh = 0.45f * m, kd = 0.9f * m;
        var kerb = Box(f, "Kerb", (k0 + k1) / 2, f.y + kh / 2, zb - kd / 2, k1 - k0, kh, kd, "panel");
        if (kerb.GetComponent<BoxCollider>() == null) { kerb.AddComponent<BoxCollider>(); kerb.layer = ObstacleLayer; solidCount++; }
        filmStructures.Add(kerb);
        Box(f, "KerbEdge", (k0 + k1) / 2, f.y + kh + 0.02f, zb - kd + 0.06f, k1 - k0, 0.04f, 0.12f, "red");
        // 5: closed emblem door under the "controls" sign at u 0.60, hazard sill, sign plate 1.6 x 0.35 m at 2.9 m
        float dx = u(0.60f), dw = 1.4f * m, dh = 2.4f * m;
        Box(f, "SignDoorFrame", dx, f.y + kh + dh / 2 + 0.03f, zb - 0.1f, dw + 0.3f, dh, 0.06f, "dark");
        Box(f, "SignDoorPanel", dx, f.y + kh + dh / 2, zb - 0.14f, dw, dh, 0.04f, "panel");
        for (int k = 0; k < 5; k++) Box(f, "SignDoorSill", dx - dw / 2 + (k + 0.5f) * dw / 5, f.y + kh + 0.12f, zb - 0.17f, dw / 5, 0.24f, 0.03f, k % 2 == 0 ? "red" : "dark");
        Box(f, "ControlsSign", dx, f.y + 2.9f * m + 0.3f, zb - 0.12f, 1.6f * m, 0.35f * m, 0.06f, "white");
        // flat dressing: notice at u 0.53, tilted recruiting poster at u 0.87
        Box(f, "NoticeA", u(0.535f), f.y + 1.8f * m + 0.9f, zb - 0.1f, 0.8f, 0.8f, 0.04f, "panel");
        var bill = Box(f, "Bill", u(0.87f), f.y + 1.6f * m + 0.6f, zb - 0.1f, 2.2f, 1.2f, 0.04f, "white");
        bill.transform.rotation = Quaternion.Euler(0f, 0f, 6f);
        BackDoorway(f, u(0.05f));
    }
    const float KerbEnd = 0.90f; // 3.5 units clear of the right-wall door (zze first pass: 0.95 failed the door gate)

    // 3: office with a long service counter, a glowing back window and a door marked by a red header.
    static void ControlsOffice(Frame f)
    {
        if (FilmRoom3Structures) { FilmOffice(f); return; }
        // round 7 FilmRedFields: the film's office is one big red field from the counter top to the ceiling (2:41)
        if (FilmRedFields) Box(f, "BackWindow", f.Cx + 3f, f.y + 3.5f, f.zb - 0.06f, 12f, 3.8f, 0.04f, "screen");
        else Box(f, "BackWindow", f.Cx + 3f, f.y + 3.2f, f.zb - 0.06f, 12f, 3.2f, 0.04f, "screen");
        Box(f, "BackWindowFrame", f.Cx + 3f, f.y + 1.5f, f.zb - 0.15f, 12.4f, 0.25f, 0.2f, "dark");
        Model(Box(f, "Counter", f.Cx + 3f, f.y + 0.6f, f.zb - 2.2f, 12f, 1.2f, 1.0f, "panel"), "nx_counter");
        Box(f, "CounterTop", f.Cx + 3f, f.y + 1.25f, f.zb - 2.2f, 12.3f, 0.1f, 1.2f, "dark");
        Box(f, "CounterStripe", f.Cx + 3f, f.y + 0.9f, f.zb - 2.72f, 12f, 0.2f, 0.02f, "red");
        Box(f, "NoticeA", f.x0 + 4f, f.y + 3.2f, f.zb - 0.1f, 1.6f, 1.1f, 0.06f, "panel");
        Box(f, "NoticeB", f.x0 + 6f, f.y + 3.0f, f.zb - 0.1f, 1.0f, 1.4f, 0.06f, "panel");
        if (FakeDoors) Box(f, "BackDoorHole", f.x0 + 9.5f, f.y + 1.9f, f.zb - 0.05f, 2.4f, 3.8f, 0.04f, "black");
        if (FakeDoors) Box(f, "BackDoorHeader", f.x0 + 9.5f, f.y + 4.0f, f.zb - 0.12f, 2.8f, 0.3f, 0.15f, "red");
        // more of the office: a door marked by a hazard header on the left, a banner, filing cabinets, a wall clock
        BackDoorway(f, f.x0 + 2.6f);
        Banner(f, f.x1 - 2.6f, f.y + 3.4f, f.zb - 0.1f, false);
        for (int i = 0; i < 3; i++) Box(f, "FileCabinet", f.x1 - 6.2f + i * 1.0f, f.y + 1.1f, f.zb - 0.55f, 0.9f, 2.2f, 0.9f, "steel");
        Cyl(f, "Clock", f.x0 + 5f, f.y + 5.2f, f.zb - 0.1f, 0.8f, 0.1f, "white");
    }

    // 4: cargo bay: strapped crates stacked against the back and a pallet line, red floor hazard band.
    static void CargoBay(Frame f)
    {
        if (FilmRoom4Structures) { FilmCargo(f); return; }
        Crate(f, f.x0 + 4f, f.zb - 1.4f, 2.2f);
        Crate(f, f.x0 + 4f, f.zb - 1.4f, 1.6f, 2.2f);
        Crate(f, f.x0 + 7f, f.zb - 1.4f, 1.8f);
        Crate(f, f.x1 - 5f, f.zb - 1.4f, 2.4f);
        Crate(f, f.x1 - 8f, f.zb - 1.2f, 1.6f);
        Box(f, "Pallet", f.Cx, f.y + 0.15f, f.zb - 1.2f, 3f, 0.3f, 2f, "steel");
        for (int i = 0; i < 12; i++)
            Box(f, "FloorHazard", f.x0 + 2f + i * 2.4f, f.y + 0.012f, f.zb - 3.3f, 1.2f, 0.02f, 0.4f, "red");
        // a wide bay: loading door at the right with hazard header, banners, more crates in the far corners
        BackDoorway(f, f.x1 - 3f);
        Banner(f, f.x0 + 0.38f * f.W, f.y + 3.6f, f.zb - 0.1f, false);
        Banner(f, f.x0 + 0.62f * f.W, f.y + 3.6f, f.zb - 0.1f, false);
        Crate(f, f.x0 + 1.6f, f.zb - 3.2f, 1.4f);
        Crate(f, f.x1 - 1.6f, f.zb - 4.4f, 1.4f);
        Box(f, "HangSign", f.Cx, f.y + 6.2f, f.zb - 3f, 3f, 0.5f, 0.1f, "red");
    }

    // ---- Film geometry, room 4 (build zza): the film's crate room, set H, 5:22.5-5:51 (structure sheet
    // ai-docs/notes/film-geometry/room-04-cargo-bay.md). 1 m = 1.5 units. The map room (about 43 units) is far
    // wider than the film's 12.5 m, so pieces keep the film's order at film size and the extra floor stays
    // bare. Solid on the one floor: the strapped crate out in the floor (u 0.475, 4 m out), the raised back
    // deck (0.45 m, 2 m deep, u 0.10-0.70) and the sloped end at its right, both solid blocks and not a route
    // (no second floor height until it is proven, prompt C1). Backdrop: the vent box on the back wall over the
    // deck, the red band with hazard stripes and an emblem plaque, the deck's red front edge.
    // Set false to build zz's cargo bay (crates against the back wall, pallet, floor strips, banners).
    const bool FilmRoom4Structures = true;
    // Build zzc (owner's play 2026-11-16: "I couldn't get up the little ramp"): the deck and its slope become the
    // chain's one walkable proof, by the recipe in ai-docs/notes/film-geometry/floor-height-research.md. The deck's
    // collider goes to layer Terrain with tag Floor (the game stands on it, waypoints snap to it); the drawn wedge
    // loses its collider and an invisible straight Box on layer Interactive (blocks standing characters only) lies
    // under it, flush with the floor at the foot and running 0.1 units into the deck top at the head, so neither end
    // has a lip. The slope is 0.675 units over 3.6, about 10.6 degrees (limit 30, game limit about 45): no
    // lengthening needed. Waypoints at the ramp's foot (floor) and head (deck), ForceConnect both ways; none over
    // the ramp (SnapToFloor ignores Interactive). The deck is 0.675 units high, far under the 2-unit link-gap and
    // fall limits, so its open front edge needs no PathBlock (a step down links like flat floor). Set false to
    // build zzb's solid deck and slope.
    const bool FilmRoom4Walkable = true;
    const int TerrainLayer = 8, InteractiveLayer = 12;
    static RoomObj curRoom;
    static void DeckRamp(Frame f, float dx1, float rl, float dh, float dd)
    {
        WalkRamp(f, "Deck", "room 4 deck", new Vector3(dx1, 0f, f.zb - dd / 2), Vector3.right, rl, dh, dd);
    }
    // zzj: DeckRamp generalised for any direction. edge = the raised floor's edge (x, z) where the ramp meets it;
    // down = the horizontal way toward the ramp's foot; width across the ramp. Same recipe as room 4: an invisible
    // straight box on layer Interactive, 0.1 into the top at the head and buried 0.2 at the foot, so neither end has a
    // lip; a waypoint a unit clear of the foot (floor) and one a unit onto the top, ForceConnect both ways.
    static void WalkRamp(Frame f, string tag, string what, Vector3 edge, Vector3 down, float rl, float dh, float width)
    {
        float ang = Mathf.Atan2(dh, rl), len = Mathf.Sqrt(rl * rl + dh * dh), th = 0.3f, over = 0.1f, bury = 0.2f;
        float full = len + (over + bury) / Mathf.Cos(ang);
        down.y = 0f; down.Normalize(); edge.y = 0f;
        var top0 = edge - down * over + Vector3.up * (f.y + dh + over * Mathf.Tan(ang));
        var top1 = edge + down * (rl + bury) + Vector3.up * (f.y - bury * Mathf.Tan(ang));
        var mid = (top0 + top1) / 2;
        var n = down * Mathf.Sin(ang) + Vector3.up * Mathf.Cos(ang); // top face normal, leaning toward the foot
        var fwd = down * Mathf.Cos(ang) - Vector3.up * Mathf.Sin(ang);
        var r = new GameObject("NX_" + tag + "RampCollider");
        r.transform.SetParent(f.Parent, true);
        r.transform.position = mid - n * (th / 2);
        r.transform.rotation = Quaternion.LookRotation(fwd, n);
        var bc = r.AddComponent<BoxCollider>();
        bc.size = new Vector3(width - 0.1f, th, full);
        r.layer = InteractiveLayer; r.isStatic = true;
        var proto = curRoom != null ? curRoom.GetComponentsInChildren<Marker_Waypoint>(true).FirstOrDefault(w => w.Type == Marker_Waypoint.MarkerType.Standard) : null;
        var parent = proto != null ? proto.transform.parent : (curRoom != null ? (curRoom.transform.Find("Waypoints") ?? curRoom.transform) : f.Parent);
        Marker_Waypoint Wp(string name, Vector3 at)
        {
            var go = proto != null ? Object.Instantiate(proto.gameObject, parent) : new GameObject(name);
            if (proto == null) { go.transform.SetParent(parent, true); go.AddComponent<Marker_Waypoint>(); }
            go.name = name; go.transform.position = at;
            var w = go.GetComponent<Marker_Waypoint>(); w.Type = Marker_Waypoint.MarkerType.Standard; w.Floating = false; w.ForceConnect = new List<Marker_Waypoint>();
            return w;
        }
        var foot = Wp("Waypoint_" + tag + "RampFoot", edge + down * (rl + 1.0f) + Vector3.up * (f.y + 0.1f));
        var head = Wp("Waypoint_" + tag + "RampHead", edge - down * 1.0f + Vector3.up * (f.y + dh + 0.1f));
        foot.ForceConnect.Add(head); head.ForceConnect.Add(foot);
        Debug.Log("NX_WALKABLE " + what + " top " + (f.y + dh).ToString("0.###") + " ramp " + (ang * Mathf.Rad2Deg).ToString("0.#") + " deg over " + rl.ToString("0.##") + " units; waypoints " + (proto != null ? "cloned" : "new") + " under " + parent.name);
    }
    // zzj: waypoints the template left inside a raised floor's footprint would sit inside its collider; lift them onto
    // the top (SnapToFloor then lands them on the Floor-tagged top). Ones over a ramp move to its foot side.
    static void LiftWaypoints(Bounds top, float y, string what)
    {
        if (curRoom == null) return;
        int k = 0;
        foreach (var w in curRoom.GetComponentsInChildren<Marker_Waypoint>(true))
        {
            var p = w.transform.position;
            if (p.x < top.min.x - 0.3f || p.x > top.max.x + 0.3f || p.z < top.min.z - 0.3f || p.z > top.max.z + 0.3f || w.name.Contains("Ramp")) continue;
            w.transform.position = new Vector3(Mathf.Clamp(p.x, top.min.x + 0.6f, top.max.x - 0.6f), y + 0.1f, Mathf.Clamp(p.z, top.min.z + 0.6f, top.max.z - 0.6f)); k++;
        }
        Debug.Log("NX_WALKABLE " + what + " waypoints lifted onto the top: " + k);
    }
    // A copy of the frame standing on a raised floor (pieces built with it start at the raised top).
    static Frame Raised(Frame f, float dh) { return new Frame { Parent = f.Parent, x0 = f.x0, x1 = f.x1, zb = f.zb, zf = f.zf, y = f.y + dh, M = f.M, Doors = f.Doors }; }
    static void FilmCargo(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f;
        // 1: strapped crate standing alone in the floor, 1.2 x 1.0 x 0.7 m
        float cx = u(0.475f), cz = f.zb - 4f * m, cw = 1.2f * m, ch = 1.0f * m, cd = 0.7f * m;
        filmStructures.Add(Box(f, "FloorCrate", cx, f.y + ch / 2, cz, cw, ch, cd, "panel"));
        Box(f, "FloorCrateLid", cx, f.y + ch + 0.03f, cz, cw + 0.06f, 0.06f, cd + 0.06f, "dark");
        foreach (var s in new[] { -0.25f, 0.25f }) Box(f, "FloorCrateStrap", cx + s * cw, f.y + ch / 2, cz, 0.16f, ch + 0.04f, cd + 0.04f, "red");
        // 2: raised back deck, 0.45 m high, 2 m deep; under Solidify's 0.8 height floor, so it gets its own collider
        float dh = 0.45f * m, dd = 2f * m, dx0 = u(0.10f), dx1 = u(0.70f);
        var deck = Box(f, "BackDeck", (dx0 + dx1) / 2, f.y + dh / 2, f.zb - dd / 2, dx1 - dx0, dh, dd, "panel");
        if (deck.GetComponent<BoxCollider>() == null) { deck.AddComponent<BoxCollider>(); deck.layer = ObstacleLayer; solidCount++; }
        if (FilmRoom4Walkable) { deck.layer = TerrainLayer; deck.tag = "Floor"; }
        filmStructures.Add(deck);
        Box(f, "BackDeckEdge", (dx0 + dx1) / 2, f.y + dh + 0.02f, f.zb - dd + 0.06f, dx1 - dx0, 0.04f, 0.12f, "red");
        Box(f, "BackDeckFace", (dx0 + dx1) / 2, f.y + dh / 2, f.zb - dd - 0.02f, dx1 - dx0, 0.12f, 0.02f, "red");
        // 3: the deck's sloped end at the right, down toward the right (a wedge turned round)
        float rl = 2.4f * m;
        var ramp = Wedge(f, "DeckSlope", dx1 + rl / 2, f.y, f.zb - dd / 2, rl, dh, dd, 0.0f, FilmRoom4Walkable ? "steel" : "panel", true);
        ramp.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        if (FilmRoom4Walkable)
        {
            // zzh (owner, glitch shot 4: "still no ramp"): the slope was panel grey on a panel grey deck and read as
            // nothing from the game camera. Steel face and the deck's red edge carried down the slope's front edge.
            float sa = Mathf.Atan2(dh, rl) * Mathf.Rad2Deg, sl = Mathf.Sqrt(rl * rl + dh * dh);
            var se = Box(f, "DeckSlopeEdge", dx1 + rl / 2, f.y + dh / 2 + 0.02f, f.zb - dd + 0.06f, sl, 0.04f, 0.12f, "red");
            se.transform.rotation = Quaternion.Euler(0f, 0f, -sa);
        }
        if (FilmRoom4Walkable) DeckRamp(f, dx1, rl, dh, dd);
        else
        {
            if (ramp.GetComponent<BoxCollider>() == null) { ramp.AddComponent<BoxCollider>(); ramp.layer = ObstacleLayer; solidCount++; }
            filmStructures.Add(ramp);
        }
        // 4: vent box on the back wall above the deck, grilled face
        float vx = u(0.45f);
        Box(f, "VentBox", vx, f.y + dh + 0.7f * m, f.zb - 0.2f, 0.7f * m, 1.4f * m, 0.3f, "steel");
        for (int k = 0; k < 6; k++) Box(f, "VentGrille", vx, f.y + dh + 0.25f * m + k * 0.18f * m, f.zb - 0.37f, 0.6f * m, 0.05f, 0.02f, "dark");
        // 5: red band with hazard stripes at 2.2 m, an emblem plaque above
        float by = f.y + 2.2f * m;
        Box(f, "HazardBand", (u(0.20f) + u(0.40f)) / 2, by, f.zb - 0.07f, u(0.40f) - u(0.20f), 0.2f * m, 0.06f, "red");
        for (float x = u(0.20f) + 0.3f; x < u(0.40f); x += 0.9f) Box(f, "HazardStripe", x, by, f.zb - 0.11f, 0.3f, 0.2f * m, 0.02f, "dark");
        Box(f, "EmblemPlaque", u(0.30f), by + 0.9f, f.zb - 0.08f, 1.2f, 0.6f, 0.05f, "red");
    }

    // 5: sniper loft: a ledge across the back wall at head height, ladder up to it, a big open-frame crate.
    static void SniperLoft(Frame f)
    {
        if (FilmRoom5Structures) { FilmLoft(f); return; }
        float ly = f.y + 3.4f, lx0 = f.Cx - 2f, lx1 = f.x1 - 1f;
        Box(f, "Ledge", (lx0 + lx1) / 2, ly, f.zb - 1.0f, lx1 - lx0, 0.3f, 2.0f, "panel");
        Box(f, "LedgeLip", (lx0 + lx1) / 2, ly + 0.05f, f.zb - 2.02f, lx1 - lx0, 0.4f, 0.06f, "red");
        Box(f, "LedgeRail", (lx0 + lx1) / 2, ly + 1.0f, f.zb - 1.95f, lx1 - lx0, 0.08f, 0.08f, "steel");
        for (float x = lx0 + 0.5f; x < lx1; x += 2.5f) Box(f, "LedgePost", x, ly + 0.55f, f.zb - 1.95f, 0.08f, 0.9f, 0.08f, "steel");
        Box(f, "LedgeSupport", lx0 + 0.3f, ly / 2 + f.y / 2, f.zb - 1.8f, 0.3f, ly - f.y, 0.3f, "dark");
        // ladder
        float lad = lx0 - 1.0f;
        Box(f, "LadderRailA", lad - 0.35f, (f.y + ly + 1f) / 2, f.zb - 1.8f, 0.08f, ly + 1f - f.y, 0.08f, "steel");
        Box(f, "LadderRailB", lad + 0.35f, (f.y + ly + 1f) / 2, f.zb - 1.8f, 0.08f, ly + 1f - f.y, 0.08f, "steel");
        for (float yy = f.y + 0.35f; yy < ly + 0.8f; yy += 0.4f) Box(f, "Rung", lad, yy, f.zb - 1.8f, 0.7f, 0.06f, 0.06f, "steel");
        // the red stripe climbs the wall beside the ledge, as in the frames
        Box(f, "BandRise", lx0 - 0.2f, (f.y + 1.6f + ly + 1.2f) / 2, f.zb - 0.07f, 0.55f, ly + 1.2f - f.y - 1.6f, 0.12f, "red");
        Box(f, "BandHigh", (lx0 + f.x1) / 2, ly + 1.0f, f.zb - 0.07f, f.x1 - lx0, 0.55f, 0.12f, "red");
        OpenCrate(f, f.x0 + 5f, f.zb - 1.8f, 2.6f);
        Banner(f, f.x0 + 0.3f * f.W, f.y + 3.4f, f.zb - 0.1f, false);
        BackDoorway(f, f.x1 - 3.5f);
    }

    // ---- Film geometry, room 5 (build zx): the film's balcony-and-ladder room, 5:51.5-6:19.5 (structure sheet
    // ai-docs/notes/film-geometry/room-05-sniper-loft.md, owner frame 6:03). 1 m = 1.5 units, as in zw. Solid on
    // the one floor: the big braced crate the player hides behind (u 0.20), the long sloped low wall running
    // diagonally out from the back wall (u 0.50 at the wall to 0.62 about 4 m out) that the guards hold, the
    // stacked crates under the balcony (u 0.72) and the wall pier beside the ladder (u 0.93). The balcony is
    // backdrop until the second floor height is researched (prompt C1): a thick slab with a chamfered underside
    // and no rail, its deck 3.6 m up, the ladder at its right end, a doorway on the deck behind the sniper, the
    // red band rising in an upright stripe to deck level. The back door moves to the film's u 0.42. Set false to
    // build zw's loft (thin railed ledge at head height, ladder at its left end, wire crate).
    const bool FilmRoom5Structures = true;
    const float LoftDeck = 5.4f, LoftSlab = 0.9f;
    static void FilmLoft(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f; // units per metre
        // 1: balcony slab, 4.5 x 1.2 x 0.6 m, the underside chamfered back to the wall (a wedge upside down)
        float bx0 = u(0.62f), bx1 = u(0.88f), deck = f.y + LoftDeck, depth = 1.2f * m;
        var slab = Wedge(f, "Ledge", (bx0 + bx1) / 2, deck - LoftSlab, f.zb - depth / 2, bx1 - bx0, LoftSlab, depth, 0.4f, "panel");
        slab.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        // 3: doorway on the deck behind the sniper, lit panel in a proud frame
        Box(f, "LoftDoorFrame", u(0.70f), deck + 1.7f, f.zb - 0.12f, 1.6f * m + 0.3f, 2.1f * m + 0.15f, 0.12f, "dark");
        Box(f, "LoftDoor", u(0.70f), deck + 1.65f, f.zb - 0.2f, 1.6f * m, 2.1f * m, 0.04f, "panel");
        // 2: ladder at the right end, from the floor past the deck
        float half = 0.85f * m / 2, lad = bx1 + half + 0.05f, top = deck + 1.2f, lz = f.zb - 0.35f;
        Box(f, "LadderRailA", lad - half, (f.y + top) / 2, lz, 0.1f, top - f.y, 0.1f, "steel");
        Box(f, "LadderRailB", lad + half, (f.y + top) / 2, lz, 0.1f, top - f.y, 0.1f, "steel");
        for (float yy = f.y + 0.45f; yy < top - 0.2f; yy += 0.45f) Box(f, "Rung", lad, yy, lz, 2 * half - 0.12f, 0.07f, 0.07f, "steel");
        // 4: wall pier right of the ladder, floor to the top of the wall; the red band wraps it
        float pw = 0.9f * m, pd = 0.5f, ph = 9f, px = u(0.93f) + 0.6f; // 0.33 m deep, not 0.5: 3.5 units clear of the right-wall door
        filmStructures.Add(Box(f, "Pier", px, f.y + ph / 2, f.zb - pd / 2, pw, ph, pd, "panel"));
        Box(f, "PierBand", px, f.y + 1.6f, f.zb - pd / 2, pw + 0.06f, 0.55f, pd + 0.06f, "red");
        Box(f, "PierBand", px, deck + 0.5f, f.zb - pd / 2, pw + 0.06f, 0.55f, pd + 0.06f, "red");
        // 5: red band turned upright left of the balcony, a second band at deck level (the pier carries the band in the recess)
        Box(f, "BandRise", u(0.60f), (f.y + 1.6f + deck + 0.75f) / 2, f.zb - 0.08f, 0.5f * m, deck + 0.75f - f.y - 1.6f, 0.12f, "red"); // zzh: front 0.02 before the wall band (owner's glitch shot 2)
        Box(f, "BandHigh", (u(0.60f) + u(0.93f)) / 2, deck + 0.5f, f.zb - 0.085f, u(0.93f) - u(0.60f) - 0.02f, 0.53f, 0.13f, "red");
        // 6: the sloped low wall, 4 m x 1.3 m x 0.45 m, diagonal from the back wall out toward the camera,
        // the slope toward the way in (left), red top edge; the guards hold behind it
        var p0 = new Vector3(u(0.50f), 0f, f.zb - 0.5f * m); var p1 = new Vector3(u(0.62f), 0f, f.zb - 4f * m);
        var mid = (p0 + p1) / 2; float yaw = Mathf.Atan2(p0.z - p1.z, p1.x - p0.x) * Mathf.Rad2Deg;
        float wl = 4f * m, wh = 1.3f * m, wt = 0.45f * m;
        var wall = Wedge(f, "SlopeWall", mid.x, f.y, mid.z, wl, wh, wt, 0.4f, "panel");
        wall.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        filmStructures.Add(wall);
        var edge = Box(f, "SlopeWallEdge", mid.x, f.y + wh + 0.04f, mid.z, wl + 0.04f, 0.08f, wt * 0.4f + 0.04f, "red");
        edge.transform.rotation = wall.transform.rotation;
        edge.transform.position = mid + wall.transform.rotation * new Vector3(0f, 0f, wt * 0.3f) + Vector3.up * (f.y + wh + 0.04f);
        // 7: the big braced crate by the way in, 2.6 m wide, 2.4 m tall, solid
        float cs = 2.4f * m, cx = u(0.20f), cz = f.zb - 2.75f * m;
        OpenCrate(f, cx, cz, cs);
        filmStructures.Add(Box(f, "CrateCore", cx, f.y + cs / 2, cz, cs - 0.2f, cs - 0.1f, cs - 0.2f, "panel")); // grey infill so the black braces read (film 5:59)
        // 8: stacked crates under the left end of the balcony, 2.1 m with 1.0 m on top
        float sx = u(0.72f), lo = 2.1f * m, hi = 1.0f * m, sz = f.zb - 1.75f * m;
        filmStructures.Add(Box(f, "CrateStack", sx, f.y + lo / 2, sz, lo, lo, lo, "panel"));
        Box(f, "CrateStackTop", sx - 0.3f, f.y + lo + hi / 2, sz - 0.3f, hi, hi, hi, "dark");
        Banner(f, f.x0 + 0.3f * f.W, f.y + 3.4f, f.zb - 0.1f, false);
        // 10: the back-wall door at the film's u 0.42 (zw had it at the right end, where the ladder now stands)
        BackDoorway(f, u(0.42f));
    }

    // 6: garage: a big roll-up shutter over a low control console, side door frame.
    static void ShutterGarage(Frame f)
    {
        float sx = f.Cx - 2f, sw = 9f, sy0 = f.y + 1.8f, sh = 4.2f;
        Box(f, "ShutterBox", sx, sy0 + sh + 0.3f, f.zb - 0.35f, sw + 0.6f, 0.6f, 0.6f, "dark");
        Box(f, "ShutterLampBar", sx, sy0 + sh - 0.05f, f.zb - 0.66f, sw, 0.14f, 0.04f, "lit"); // bright accent
        for (float yy = sy0; yy < sy0 + sh; yy += 0.3f)
            Box(f, "Slat", sx, yy + 0.13f, f.zb - 0.15f, sw, 0.24f, 0.08f, "steel");
        Box(f, "ShutterGuideA", sx - sw / 2 - 0.1f, sy0 + sh / 2, f.zb - 0.2f, 0.2f, sh + 0.4f, 0.25f, "dark");
        Box(f, "ShutterGuideB", sx + sw / 2 + 0.1f, sy0 + sh / 2, f.zb - 0.2f, 0.2f, sh + 0.4f, 0.25f, "dark");
        Model2(Box(f, "Console", sx, f.y + 0.8f, f.zb - 0.6f, sw - 1f, 1.6f, 0.9f, "panel"), "nx_console");
        for (int i = 0; i < 4; i++) Box(f, "ConsoleLamp", sx - 3f + i * 2f, f.y + 1.0f, f.zb - 1.07f, 0.8f, 0.6f, 0.03f, i == 3 ? "screen" : "dark");
        Box(f, "WallPanel", f.x1 - 4f, f.y + 3.2f, f.zb - 0.1f, 2.2f, 3.0f, 0.08f, "dark");
        Box(f, "WallPanelStripes", f.x1 - 4f, f.y + 2.0f, f.zb - 0.15f, 2.2f, 0.3f, 0.04f, "red");
        // vending machine and a banner beside the shutter, oil stains on the floor
        Model2(Box(f, "Vending", f.x0 + 2.4f, f.y + 1.9f, f.zb - 0.7f, 1.6f, 3.8f, 1.2f, "dark"), "nx_vending");
        Box(f, "VendingFace", f.x0 + 2.4f, f.y + 2.3f, f.zb - 1.32f, 1.2f, 2.4f, 0.03f, "screen");
        Banner(f, f.x1 - 7f, f.y + 3.4f, f.zb - 0.1f, false);
        foreach (var x in new[] { 0.3f, 0.55f, 0.75f }) Box(f, "OilStain", f.x0 + x * f.W, f.y + 0.006f, f.Cz + (x - 0.5f) * 3f, 1.6f, 0.006f, 1.0f, "stain");
        // owner's film frame (2026-11-11): tapered concrete pylons with a red band, a generator set on the back wall
        if (FilmRoom6Structures) PillarHall(f);
        else
        {
            Pylon(f, f.x0 + 0.22f * f.W, f.zf + 0.38f * f.D);
            Pylon(f, f.x0 + 0.46f * f.W, f.zf + 0.30f * f.D);
        }
        Generator(f, f.x1 - 4.4f, f.zb - 0.95f);
    }

    // ---- Film geometry, room 6 (build zw): the film's pillar-and-pylon hall, 8:42-9:37 (structure sheet
    // ai-docs/notes/film-geometry/room-06-shutter-garage.md). Solid pieces on the one floor only: the square
    // pillar with a sloped barricade touching it on each side, a low barricade out on the floor, two tall
    // sloped blocks with a red top edge, and the two pylons at film size (far 2.9 m, near 2.3 m). One grunt
    // (1.8 m) is 2.7 units here, so 1 m = 1.5 units. The stepped platform and its striped steps are left out
    // until the game's second floor height is researched (prompt C1). Order left to right is the film's
    // (pillar line, sloped blocks, pylons, generator); the shutter and console sit between pillar and blocks.
    // The engineers at the console end up between and behind the blocks and the far pylon, where the film
    // has its soldiers. Set false to build zv's room 6 (two small pylons).
    const bool FilmRoom6Structures = true;
    static readonly List<GameObject> filmStructures = new List<GameObject>();
    static void PillarHall(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        // 1-2: pillar (0.6 m shaft on a 0.75 m plinth with two red bands), clear of the back wall, barricades each side
        float px = u(0.20f), pz = f.zb - 2.4f;
        filmStructures.Add(Box(f, "PillarPlinth", px, f.y + 0.53f, pz, 1.15f, 1.05f, 1.15f, "dark"));
        Box(f, "PillarBand", px, f.y + 0.75f, pz, 1.19f, 0.14f, 1.19f, "red");
        Box(f, "PillarBand", px, f.y + 0.35f, pz, 1.19f, 0.14f, 1.19f, "red");
        filmStructures.Add(Box(f, "Pillar", px, f.y + 4.5f, pz, 0.9f, 9.0f, 0.9f, "panel"));
        foreach (var side in new[] { -1f, 1f })
            Barricade(f, px + side * (0.575f + 1.0f), pz, 2.0f, 1.2f, 0.75f);
        // 3: low barricade out on the near floor
        Barricade(f, u(0.31f), f.zb - 6.0f, 1.35f, 1.05f, 0.7f);
        // 4: tall sloped blocks right of the console, one near the back wall, one about 4 m out
        foreach (var d in new[] { 1.9f, 7.2f })
        {
            // long front to back, the slope facing left toward the pillar (8:49 frame), red edge along the ridge
            filmStructures.Add(Wedge(f, "SlopeBlock", u(0.595f), f.y, f.zb - d, 1.35f, 3.3f, 2.4f, 0.3f, "panel", true));
            Box(f, "SlopeBlockEdge", u(0.595f) + 0.675f - 0.2f, f.y + 3.3f + 0.04f, f.zb - d, 0.44f, 0.1f, 2.44f, "red");
        }
        // 8-9: the pylons at film size, far near the back wall, near one in front of it
        // the pylon body starts on its 0.24 plinth, so Solidify (floor pieces only) never gave zv's pylons a
        // collider: give the film pylons one over plinth and body, as tall as the body
        var pf = f;
        if (FilmRoom6Ramp) pf = PylonDeck(f);
        foreach (var py in new[] { Pylon(pf, u(0.70f), f.zb - 2.5f, 4.1f, 3.0f, 2.2f), Pylon(pf, u(0.765f), f.zb - 8.6f, 3.2f, 2.7f, 2.0f) }) // zzh: near pylon 1.6 further out, a 3-unit way between them (glitch shot 4)
        {
            if (py == null) continue;
            var bc = py.AddComponent<BoxCollider>();
            bc.center = new Vector3(0f, -0.12f / py.transform.localScale.y, 0f);
            bc.size = new Vector3(1f, 1f + 0.24f / py.transform.localScale.y, 1f);
            py.layer = ObstacleLayer; solidCount++;
            filmStructures.Add(py);
        }
    }

    // Build zzj (owner, after playing zzh: "no ramp up to the pylons"): the film's raised platform under both pylons
    // (frames t534 = 8:54 and t541 = 9:01 in the lab's film-geometry/height-zzi/) with hazard-striped steps down its
    // left flank between the two tall sloped blocks. Built as one walkable floor 0.675 units high (room 4's proven
    // height and recipe, floor-height-research.md): u 0.62-0.80, from the back wall to 10 units out, collider on layer
    // Terrain with tag Floor; the pylons stand on it; a striped walkable ramp (14 degrees, 2.7 units, as room 8's
    // steps) runs down its left flank toward the console, 3.2 to 5.9 units out, in the gap between the sloped blocks.
    // The film's lower tier is merged into the one height: the game has no step-up, so every lip needs its own ramp.
    // Its open front and right edges are 0.675 high, under the 2-unit link gap, so no PathBlock. The engineers move a
    // little left (u 0.45, 0.51) to stay clear of the ramp's foot; spike strip B moves out (v 0.4) off the platform.
    // Set false to build zzi's room 6 (pylons on the floor).
    const bool FilmRoom6Ramp = true;
    static Frame PylonDeck(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float dh = 0.675f, x0 = u(0.62f), x1 = u(0.80f), depth = 10f, zc = f.zb - depth / 2;
        var deck = Box(f, "PylonDeck", (x0 + x1) / 2, f.y + dh / 2, zc, x1 - x0, dh, depth, "panel");
        if (deck.GetComponent<BoxCollider>() == null) { deck.AddComponent<BoxCollider>(); solidCount++; }
        deck.layer = TerrainLayer; deck.tag = "Floor";
        filmStructures.Add(deck);
        // red top edge along the front and the left flank, hazard stripes on the front face
        Box(f, "PylonDeckEdge", (x0 + x1) / 2, f.y + dh + 0.02f, f.zb - depth + 0.06f, x1 - x0, 0.04f, 0.12f, "red");
        Box(f, "PylonDeckEdge", x0 + 0.06f, f.y + dh + 0.02f, zc + 0.06f, 0.12f, 0.04f, depth - 0.12f, "red"); // stops at the front edge (S26)
        for (float x = x0 + 0.3f; x < x1 - 0.2f; x += 0.9f) Box(f, "PylonDeckStripe", x, f.y + dh / 2, f.zb - depth - 0.02f, 0.35f, dh - 0.1f, 0.02f, "red");
        // the ramp down the left flank: drawn wedge (no collider), stripes across it, the walkable box under it
        float rl = 2.7f, rz0 = 3.2f, rz1 = 5.9f, rw = rz1 - rz0, rzc = f.zb - (rz0 + rz1) / 2, ang = Mathf.Atan2(dh, rl);
        var ramp = Wedge(f, "PylonRamp", x0 - rl / 2, f.y, rzc, rl, dh, rw, 0.0f, "steel", true);
        var rc = ramp.GetComponent<Collider>(); if (rc != null) Object.DestroyImmediate(rc);
        filmStructures.Add(ramp);
        for (int k = 0; k < 5; k++)
        {
            float t = (k + 0.5f) / 5f, x = x0 - rl + t * rl;
            var st = Box(f, "PylonRampStripe", x, f.y + dh * t + 0.03f, rzc, 0.28f, 0.03f, rw - 0.1f, k % 2 == 0 ? "red" : "dark");
            st.transform.rotation = Quaternion.Euler(0f, 0f, ang * Mathf.Rad2Deg);
        }
        WalkRamp(f, "Pylon", "room 6 pylon deck", new Vector3(x0, 0f, rzc), Vector3.left, rl, dh, rw);
        LiftWaypoints(deck.GetComponent<Renderer>().bounds, f.y + dh, "room 6 pylon deck");
        // waypoints over the ramp would snap under it (SnapToFloor ignores Interactive): move them past its foot
        if (curRoom != null)
            foreach (var w in curRoom.GetComponentsInChildren<Marker_Waypoint>(true))
            {
                var p = w.transform.position;
                if (w.name.Contains("Ramp") || p.x < x0 - rl - 0.3f || p.x > x0 || p.z < f.zb - rz1 - 0.3f || p.z > f.zb - rz0 + 0.3f) continue;
                w.transform.position = new Vector3(x0 - rl - 1.2f, p.y, p.z);
                Debug.Log("NX_WALKABLE room 6 waypoint " + w.name + " moved off the ramp");
            }
        return Raised(f, dh);
    }

    // Low wall with a sloped front face and a red top edge (the film's barricades).
    static void Barricade(Frame f, float x, float z, float len, float h, float thick)
    {
        filmStructures.Add(Wedge(f, "Barricade", x, f.y, z, len, h, thick, 0.45f, "panel"));
        Box(f, "BarricadeEdge", x, f.y + h + 0.03f, z + thick * 0.275f, len + 0.04f, 0.08f, thick * 0.45f + 0.04f, "red");
    }

    // A box whose front face (toward the camera, -z) slopes from the full base back to a top of topFrac depth;
    // the collider stays the base box Solidify gives it, as with Taper.
    static GameObject Wedge(Frame f, string name, float x, float y0, float z, float sx, float h, float sz, float topFrac, string mat, bool slopeLeft = false)
    {
        var g = Box(f, name, x, y0 + h / 2, z, sx, h, sz, mat);
        g.GetComponent<MeshFilter>().sharedMesh = WedgeMesh(Mathf.Round(topFrac * 100f) / 100f, slopeLeft);
        return g;
    }

    static readonly Dictionary<float, Mesh> wedges = new Dictionary<float, Mesh>();
    static Mesh WedgeMesh(float k, bool left)
    {
        float key = left ? -k : k;
        if (wedges.TryGetValue(key, out var m) && m != null) return m;
        string tag = (left ? "L" : "") + Mathf.RoundToInt(k * 100);
        string path = MatDir + "/NX_Wedge_" + tag + ".asset";
        m = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (m == null)
        {
            float zt = 0.5f - k;
            var bot = new[] { new Vector3(-.5f, -.5f, -.5f), new Vector3(.5f, -.5f, -.5f), new Vector3(.5f, -.5f, .5f), new Vector3(-.5f, -.5f, .5f) };
            var top = left ? new[] { new Vector3(zt, .5f, -.5f), new Vector3(.5f, .5f, -.5f), new Vector3(.5f, .5f, .5f), new Vector3(zt, .5f, .5f) }
                : new[] { new Vector3(-.5f, .5f, zt), new Vector3(.5f, .5f, zt), new Vector3(.5f, .5f, .5f), new Vector3(-.5f, .5f, .5f) };
            var v = new List<Vector3>(); var tri = new List<int>();
            void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d) { int i = v.Count; v.Add(a); v.Add(b); v.Add(c); v.Add(d); tri.AddRange(new[] { i, i + 1, i + 2, i, i + 2, i + 3 }); }
            for (int i = 0; i < 4; i++) { int j = (i + 1) % 4; Quad(bot[i], top[i], top[j], bot[j]); }
            Quad(top[0], top[3], top[2], top[1]);
            Quad(bot[0], bot[1], bot[2], bot[3]);
            m = new Mesh { name = "NX_Wedge_" + tag };
            m.SetVertices(v); m.SetTriangles(tri, 0);
            m.uv = v.Select(q => new Vector2(q.x + .5f, q.y + .5f)).ToArray();
            m.RecalculateNormals(); m.RecalculateBounds();
            AssetDatabase.CreateAsset(m, path);
        }
        wedges[key] = m;
        return m;
    }

    // Gate log for the film structures (prompt C4): no structure within 3.5 units of a door, every enemy, cover
    // point and the player start at least 1.2 units clear of every structure's footprint, and an open band at
    // least 6 units deep in front of them from wall to wall (the route). Prints NX_STRUCT lines.
    static void CheckFilmStructures(List<RoomObj> built)
    {
        foreach (var room in built)
        {
            var mine = filmStructures.Where(g => g != null && g.transform.IsChildOf(room.transform)).ToList();
            if (mine.Count == 0) continue;
            var f = frames[room]; int bad = 0;
            var chars = room.GetComponentsInChildren<Marker_Character>(true).Select(c => c.transform.position).ToList();
            var feet = room.GetComponentsInChildren<Marker_Character>(true).Select(c => new KeyValuePair<string, Vector3>(c.name, c.transform.position))
                .Concat(room.GetComponentsInChildren<ProxyObj_Cover>(true).Select(c => new KeyValuePair<string, Vector3>(c.name, c.transform.position))).ToList();
            foreach (var g in mine)
            {
                var b = g.GetComponent<Renderer>().bounds;
                Debug.Log("NX_STRUCT " + room.name + " " + g.name + " h " + (b.size.y / 2.7f).ToString("0.00") + " at x " + (b.center.x - f.x0).ToString("0.0")
                    + " out " + (f.zb - b.center.z).ToString("0.0") + " solid " + (g.GetComponent<Collider>() != null));
                foreach (var d in f.Doors)
                {
                    var gap = new Vector2(Mathf.Max(0f, Mathf.Max(b.min.x - d.x, d.x - b.max.x)), Mathf.Max(0f, Mathf.Max(b.min.z - d.z, d.z - b.max.z)));
                    if (gap.magnitude < 3.5f) { bad++; Debug.Log("NX_STRUCT_BLOCK door " + room.name + " " + g.name + " near door at " + d); }
                }
                var e = b; e.Expand(new Vector3(2.4f, 0f, 2.4f)); // a cover proxy's own footprint plus a step
                var ec = b; ec.Expand(new Vector3(1.2f, 0f, 1.2f)); // a character's width plus a step
                foreach (var c in chars)
                    if (c.x > ec.min.x && c.x < ec.max.x && c.z > ec.min.z && c.z < ec.max.z) { bad++; Debug.Log("NX_STRUCT_BLOCK floor " + room.name + " " + g.name + " x enemy at " + c); }
                foreach (var p in feet.Where(q => q.Key.StartsWith("PX_Cover")))
                    if (p.Value.x > e.min.x && p.Value.x < e.max.x && p.Value.z > e.min.z && p.Value.z < e.max.z) { bad++; Debug.Log("NX_STRUCT_BLOCK floor " + room.name + " " + g.name + " x " + p.Key); }
            }
            float front = mine.Min(g => g.GetComponent<Renderer>().bounds.min.z) - FloorBounds(room).min.z;
            if (front < 6f) { bad++; Debug.Log("NX_STRUCT_BLOCK route " + room.name + " open depth " + front.ToString("0.0")); }
            Debug.Log("NX_STRUCT_GATE " + room.name + " structures " + mine.Count + " enemies " + chars.Count + " covers " + feet.Count(q => q.Key.StartsWith("PX_Cover")) + " blocks " + bad + " open front depth " + front.ToString("0.0"));
        }
    }

    // Tapered concrete pylon (the film's checkpoint blocks): one sloped body on a plinth, a red band and white stripe
    // that follow the slope, a dark cap. Owner asked for real pylons (2026-11-11 play): the four stacked boxes read as crates.
    static void Pylon(Frame f, float x, float z) { Pylon(f, x, z, 2.6f, 1.7f, 0.8f); }
    static GameObject Pylon(Frame f, float x, float z, float H, float wb, float wt)
    {
        if (f.Doors.Any(d => Mathf.Abs(d.x - x) < 2.5f && Mathf.Abs(d.z - z) < 2.5f)) return null;
        Box(f, "Pylon", x, f.y + 0.12f, z, wb + 0.2f, 0.24f, wb + 0.2f, "dark");
        float y0 = f.y + 0.24f, k = H / 2.6f;
        var body = Taper(f, "PylonTier", x, y0, z, wb, wt, H, "panel");
        // bands sit a hair proud of the slope at their own height
        Taper(f, "PylonTier", x, y0 + 0.55f * k, z, W(wb, wt, H, 0.55f * k) + 0.04f, W(wb, wt, H, 1.15f * k) + 0.04f, 0.6f * k, "red");
        Taper(f, "PylonTier", x, y0 + 1.25f * k, z, W(wb, wt, H, 1.25f * k) + 0.04f, W(wb, wt, H, 1.4f * k) + 0.04f, 0.15f * k, "white");
        Box(f, "PylonCap", x, y0 + H + 0.06f, z, wt + 0.1f, 0.12f, wt + 0.1f, "dark");
        Box(f, "PylonTier", x, y0 + H + 0.2f, z, 0.25f, 0.16f, 0.25f, "steel");
        return body;
    }

    static float W(float wb, float wt, float H, float h) { return Mathf.Lerp(wb, wt, h / H); }

    // A box whose mesh is swapped for a square frustum (bottom width wb, top width wt); the collider stays the
    // bottom-sized box Solidify gives it, which is what a player bumps into anyway.
    static GameObject Taper(Frame f, string name, float x, float y0, float z, float wb, float wt, float h, string mat)
    {
        var g = Box(f, name, x, y0 + h / 2, z, wb, h, wb, mat);
        g.GetComponent<MeshFilter>().sharedMesh = FrustumMesh(Mathf.Round(wt / wb * 100f) / 100f);
        return g;
    }

    static readonly Dictionary<float, Mesh> frustums = new Dictionary<float, Mesh>();
    static Mesh FrustumMesh(float r)
    {
        if (frustums.TryGetValue(r, out var m) && m != null) return m;
        string path = MatDir + "/NX_Frustum_" + Mathf.RoundToInt(r * 100) + ".asset";
        m = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (m == null)
        {
            // unit cube from -0.5..0.5, top face scaled by r; flat-shaded (own vertices per face)
            float t = 0.5f * r;
            var bot = new[] { new Vector3(-.5f, -.5f, -.5f), new Vector3(.5f, -.5f, -.5f), new Vector3(.5f, -.5f, .5f), new Vector3(-.5f, -.5f, .5f) };
            var top = new[] { new Vector3(-t, .5f, -t), new Vector3(t, .5f, -t), new Vector3(t, .5f, t), new Vector3(-t, .5f, t) };
            var v = new List<Vector3>(); var tri = new List<int>();
            void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d) { int i = v.Count; v.Add(a); v.Add(b); v.Add(c); v.Add(d); tri.AddRange(new[] { i, i + 1, i + 2, i, i + 2, i + 3 }); }
            for (int i = 0; i < 4; i++) { int j = (i + 1) % 4; Quad(bot[i], top[i], top[j], bot[j]); }
            Quad(top[0], top[3], top[2], top[1]);
            Quad(bot[0], bot[1], bot[2], bot[3]);
            m = new Mesh { name = "NX_Frustum_" + Mathf.RoundToInt(r * 100) };
            m.SetVertices(v); m.SetTriangles(tri, 0);
            m.uv = v.Select(p => new Vector2(p.x + .5f, p.y + .5f)).ToArray();
            m.RecalculateNormals(); m.RecalculateBounds();
            AssetDatabase.CreateAsset(m, path);
        }
        frustums[r] = m;
        return m;
    }

    // Petrol generator set on the back wall (film frame): red body on a skid with a fuel tank on top, control face
    // with dials, grille, exhaust stack up the wall, wheels; gasoline can, hazard sign, hose, toolbox and wrench.
    static void Generator(Frame f, float x, float z)
    {
        float y = f.y;
        Box(f, "GeneratorFoot", x, y + 0.1f, z, 3.2f, 0.2f, 1.4f, "black");
        Box(f, "Generator", x, y + 0.95f, z, 3.0f, 1.5f, 1.25f, "red");
        Box(f, "GeneratorFrame", x, y + 1.75f, z, 3.1f, 0.1f, 1.35f, "dark");
        Box(f, "GeneratorTank", x - 0.3f, y + 2.05f, z, 1.8f, 0.5f, 0.95f, "red");
        Box(f, "GeneratorTankCap", x - 0.9f, y + 2.36f, z, 0.25f, 0.12f, 0.25f, "steel");
        foreach (var cx in new[] { -1.55f, 1.55f }) Box(f, "GeneratorFrame", x + cx, y + 1.3f, z - 0.6f, 0.1f, 2.2f, 0.1f, "dark");
        Box(f, "GeneratorFace", x + 0.55f, y + 1.05f, z - 0.64f, 1.4f, 0.8f, 0.03f, "white");
        for (int i = 0; i < 3; i++) Box(f, "GeneratorSwitch", x + 0.1f + i * 0.45f, y + 1.15f, z - 0.67f, 0.26f, 0.26f, 0.03f, i == 2 ? "screen" : "dark");
        Box(f, "GeneratorSwitch", x + 0.55f, y + 0.8f, z - 0.67f, 1.1f, 0.12f, 0.03f, "dark");
        for (int i = 0; i < 5; i++) Box(f, "GeneratorFace", x - 0.85f, y + 0.55f + i * 0.17f, z - 0.64f, 1.0f, 0.07f, 0.03f, "dark");
        foreach (var cx in new[] { -1.2f, 1.2f }) Box(f, "GeneratorFoot", x + cx, y + 0.25f, z - 0.72f, 0.5f, 0.5f, 0.2f, "black");
        Box(f, "GeneratorExhaust", x - 1.3f, y + 3.0f, f.zb - 0.2f, 0.22f, 2.6f, 0.22f, "steel");
        Box(f, "GeneratorExhaust", x - 1.3f, y + 1.9f, z + 0.1f, 0.22f, 0.22f, f.zb - 0.2f - z, "steel");
        Box(f, "GasCan", x + 2.1f, y + 0.5f, z - 0.1f, 0.6f, 1.0f, 0.4f, "white");
        Box(f, "GasCanLabel", x + 2.1f, y + 0.5f, z - 0.31f, 0.42f, 0.45f, 0.02f, "red");
        Box(f, "GasCanSpout", x + 2.22f, y + 1.1f, z - 0.1f, 0.07f, 0.25f, 0.07f, "steel");
        Box(f, "HazardSign", x + 3.1f, y + 2.8f, f.zb - 0.08f, 1.0f, 1.0f, 0.04f, "dark");
        Box(f, "HazardSignMark", x + 3.1f, y + 2.85f, f.zb - 0.11f, 0.55f, 0.55f, 0.02f, "screen");
        Box(f, "HoseHook", x + 2.2f, y + 2.6f, f.zb - 0.12f, 0.12f, 0.12f, 0.15f, "steel");
        Box(f, "HoseCoil", x + 2.2f, y + 2.05f, f.zb - 0.12f, 0.6f, 1.1f, 0.08f, "black");
        Box(f, "Toolbox", x - 3.2f, y + 0.2f, f.zf + 0.35f * f.D, 0.9f, 0.4f, 0.45f, "red");
        Box(f, "ToolboxHandle", x - 3.2f, y + 0.42f, f.zf + 0.35f * f.D, 0.4f, 0.04f, 0.06f, "white");
        Box(f, "Wrench", x - 2.4f, y + 0.02f, f.zf + 0.33f * f.D, 1.0f, 0.04f, 0.12f, "steel");
    }

    // 7: corridor lit by ceiling lamps throwing red cones down the back wall; dark slate board.
    static void SpotlightCorridor(Frame f)
    {
        // zzf FilmRoom7Structures: the film's two cones at u 0.20 and 0.65, not four spread to the ends
        float[] lampAt = FilmRoom7Structures ? new[] { f.x0 + 0.20f * f.W, f.x0 + 0.65f * f.W } : new[] { 0f, 1f, 2f, 3f }.Select(i => f.x0 + 4f + i * (f.W - 8f) / 3f).ToArray();
        for (int i = 0; i < lampAt.Length; i++)
        {
            float x = lampAt[i];
            Box(f, "Lamp", x, f.y + 6.3f, f.zb - 0.5f, 1.4f, 0.25f, 0.6f, "dark");
            var glow = Box(f, "LampGlow", x, f.y + 6.16f, f.zb - 0.5f, 1.1f, 0.04f, 0.4f, "screen");
            var dead = Box(f, "LampDead", x, f.y + 6.155f, f.zb - 0.5f, 1.08f, 0.04f, 0.38f, "black"); // zzh: a hair inside the glow (S26)
            var beam = new GameObject("Beam");
            beam.transform.SetParent(f.Parent, false);
            beam.transform.position = new Vector3(x, f.y, f.zb - 0.5f);
            beam.transform.localScale = new Vector3(5f, 6.2f, 3f); // zzh: a volume; its back half is inside the wall
            beam.AddComponent<MeshFilter>().sharedMesh = ConeMesh();
            var r = beam.AddComponent<MeshRenderer>();
            r.sharedMaterial = f.M["beam"];
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
            // shoot the lamp out: its glow and cone go dark
            Breakable(f, "Lamp" + i, new Vector3(x, f.y + 6.2f, f.zb - 0.5f), new Vector3(1.4f, 0.4f, 0.8f),
                new List<GameObject> { glow, beam }, new List<GameObject> { dead }, LampHits, "", LampBreakSound);
        }
        if (FilmRoom7Structures) { FilmCorridor(f); return; }
        Box(f, "Slate", f.Cx + 1f, f.y + 3.2f, f.zb - 0.1f, 6f, 2.4f, 0.06f, "dark");
        Box(f, "SlateFrame", f.Cx + 1f, f.y + 1.95f, f.zb - 0.14f, 6.3f, 0.12f, 0.12f, "steel");
        Box(f, "Shutter", f.x0 + 3f, f.y + 2.2f, f.zb - 0.12f, 2f, 4.4f, 0.06f, "steel");
        for (float yy = f.y + 0.3f; yy < f.y + 4.4f; yy += 0.35f) Box(f, "ShutterLine", f.x0 + 3f, yy, f.zb - 0.16f, 2f, 0.05f, 0.02f, "dark");
        // banners between the lamps and doorways at both ends of the long corridor
        for (int i = 0; i < 3; i++) { float bx = f.x0 + 4f + (i + 0.5f) * (f.W - 8f) / 3f; if (Mathf.Abs(bx - (f.Cx + 1f)) > 4.2f) Banner(f, bx, f.y + 3.4f, f.zb - 0.1f, false); }
        BackDoorway(f, f.x0 + 1.9f);
        BackDoorway(f, f.x1 - 1.9f);
    }

    // ---- Film geometry, room 7 (build zzf): the film's spotlight corridor, 6:40.5-7:25.5 (structure sheet
    // ai-docs/notes/film-geometry/room-07-spotlight-corridor.md). 1 m = 1.5 units. The film's floor is flat: cover is
    // the wall, so the only solid pieces are the grey wall blocks framing the red wall at both ends (0.4 m proud,
    // left 0.8 m and right 1.4 m wide, full height). Backdrop: the two lamps and cones (SpotlightCorridor), the framed
    // screen panel u 0.25-0.55 (bottom 1.8 m up), the emblem door at u 0.63 shown closed with its hazard foot (it
    // opens in the film; fake doors stay off), the louvre panel at u 0.10. Left out: the right block's dark doorway
    // (fake doors off) and the hip-height red band (the band is shared wall dressing; a change for every room).
    // Removed under the toggle: two of four lamps, the slate, banners. Set false to build zze's corridor.
    const bool FilmRoom7Structures = true;
    static void FilmCorridor(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f, zb = f.zb, pr = 0.4f * m;
        // 5: grey wall blocks at both ends, proud of the red wall; 3.6 units in from the side walls, clear of the
        // side-wall doors 3.1 units out (zzf first pass: blocks at the corners failed the door gate)
        foreach (var (x, w) in new[] { (f.x0 + 3.6f + 0.4f * m, 0.8f * m), (f.x1 - 3.6f - 0.7f * m, 1.4f * m) })
        {
            filmStructures.Add(Box(f, "EndBlock", x, f.y + 4.5f, zb - pr / 2, w, 9f, pr, "wall"));
            Box(f, "EndBlockSkirt", x, f.y + 0.2f, zb - pr / 2, w + 0.06f, 0.4f, pr + 0.06f, "red");
        }
        // 2: framed screen panel u 0.25-0.55, 0.9 m tall, bottom 1.8 m up
        float s0 = u(0.25f), s1 = u(0.55f), sy = f.y + 1.8f * m, sh = 0.9f * m;
        Box(f, "Slate", (s0 + s1) / 2, sy + sh / 2, zb - 0.1f, s1 - s0, sh, 0.06f, "dark");
        Box(f, "SlateFrame", (s0 + s1) / 2, sy - 0.06f, zb - 0.14f, s1 - s0 + 0.2f, 0.12f, 0.12f, "steel");
        Box(f, "SlateText", s1 - 1.2f, sy + 0.3f, zb - 0.14f, 1.6f, 0.25f, 0.02f, "white");
        // 3: emblem door at u 0.63, 1.1 x 2.0 m, closed, hazard-striped foot
        float dx = u(0.63f), dw = 1.1f * m, dh = 2.0f * m;
        Box(f, "EmblemDoorFrame", dx, f.y + dh / 2 + 0.05f, zb - 0.08f, dw + 0.3f, dh + 0.1f, 0.05f, "dark");
        Box(f, "EmblemDoorPanel", dx, f.y + dh / 2 + 0.05f, zb - 0.12f, dw, dh, 0.04f, "panel");
        Box(f, "EmblemDoorMark", dx, f.y + dh * 0.6f, zb - 0.15f, 0.7f, 0.7f, 0.02f, "red");
        for (int k = 0; k < 4; k++) Box(f, "EmblemDoorFoot", dx - dw / 2 + (k + 0.5f) * dw / 4, f.y + 0.2f, zb - 0.15f, dw / 4, 0.3f, 0.02f, k % 2 == 0 ? "red" : "dark");
        // 6: louvre panel at u 0.10, 1.0 x 2.5 m
        float lx = f.x0 + 3.6f + 0.8f * m + 0.2f + 0.5f * m, lh = 2.5f * m; // right of the left block (film u 0.05-0.15)
        Box(f, "Shutter", lx, f.y + lh / 2 + 0.05f, zb - 0.12f, 1.0f * m, lh, 0.06f, "steel");
        for (float yy = f.y + 0.3f; yy < f.y + lh; yy += 0.3f) Box(f, "ShutterLine", lx, yy, zb - 0.16f, 1.0f * m, 0.05f, 0.02f, "dark");
    }

    // 8: locker room: a bank of tall lockers, a bench, a cabinet; a ceiling lamp cone.
    static void LockerRoom(Frame f)
    {
        if (FilmRoom8Structures) { FilmLockers(f); return; }
        float lx = f.x0 + 4f;
        for (int i = 0; i < 6; i++)
        {
            float x = lx + i * 1.15f;
            Model(Box(f, "Locker", x, f.y + 1.7f, f.zb - 0.6f, 1.1f, 3.4f, 1.0f, "steel"), "nx_locker");
            for (int v = 0; v < 3; v++) Box(f, "Vent", x, f.y + 2.9f - v * 0.18f, f.zb - (ModelledProps ? 1.16f : 1.12f), 0.6f, 0.06f, 0.02f, "dark");
            Box(f, "Handle", x + 0.35f, f.y + 1.8f, f.zb - (ModelledProps ? 1.16f : 1.12f), 0.06f, 0.35f, 0.04f, "dark");
        }
        Box(f, "LockerStripLight", lx + 2.9f, f.y + 3.75f, f.zb - 0.5f, 6.6f, 0.16f, 0.3f, "lit"); // bright accent
        Model(Box(f, "Bench", lx + 3f, f.y + 0.5f, f.zb - 2.4f, 5f, 0.18f, 0.7f, "dark"), "nx_bench");
        Box(f, "BenchLegA", lx + 1f, f.y + 0.25f, f.zb - 2.4f, 0.15f, 0.5f, 0.5f, "dark");
        Box(f, "BenchLegB", lx + 5f, f.y + 0.25f, f.zb - 2.4f, 0.15f, 0.5f, 0.5f, "dark");
        Model(Box(f, "Cabinet", f.x1 - 5f, f.y + 1.1f, f.zb - 0.6f, 2.4f, 2.2f, 1.0f, "panel"), "nx_cabinet");
        Box(f, "BoardFrame", f.x1 - 9f, f.y + 3.3f, f.zb - 0.1f, 1.4f, 1.8f, 0.1f, "red");
        Box(f, "Board", f.x1 - 9f, f.y + 3.3f, f.zb - 0.16f, 1.1f, 1.5f, 0.04f, "panel");
        Banner(f, f.x1 - 2.2f, f.y + 3.4f, f.zb - 0.1f, false);
        Cyl(f, "Bin", f.x1 - 7f, f.y + 0.6f, f.zb - 0.7f, 0.9f, 1.2f, "steel");
    }

    // ---- Film geometry, room 8 (build zz): the film's locker room, 7:59.5-8:31.5 (structure sheet
    // ai-docs/notes/film-geometry/room-08-locker-room.md). 1 m = 1.5 units. The map room (about 23 units) is
    // near the film's 16 m, so pieces stand at the film's across positions and sizes. Solid on the one floor:
    // three tall lockers (u 0.18-0.33), the square column (u 0.36), the wall cabinet (u 0.62), the free-standing
    // cabinet and crate (u 0.70, 0.77; the right half's cover) and the flight of steps with its landing at the
    // right, built as a solid stepped block, not a route (no second floor height until it is proven, prompt
    // C1). Backdrop: the emblem door in the back wall, the two red spotlight cones (breakable, as room 7's
    // lamps), the wall box on its pipe. Set false to build zy's locker room (six lockers, bench, board).
    const bool FilmRoom8Structures = true;
    static void FilmLockers(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f;
        // 2: three tall lockers, 2.4 m tall, each 0.67 m wide, 0.5 m deep, vents at the top
        for (int i = 0; i < 3; i++)
        {
            float x = u(0.205f) + i * 0.67f * m;
            filmStructures.Add(Model(Box(f, "Locker", x, f.y + 1.2f * m, f.zb - 0.25f * m - 0.05f, 0.65f * m, 2.4f * m, 0.5f * m, "steel"), "nx_locker"));
            for (int v = 0; v < 3; v++) Box(f, "Vent", x, f.y + 2.1f * m - v * 0.18f, f.zb - 0.5f * m - 0.12f, 0.6f, 0.06f, 0.02f, "dark");
        }
        // 3: square column just proud of the back wall, floor to ceiling
        filmStructures.Add(Box(f, "Column", u(0.36f), f.y + 4.5f, f.zb - 0.225f * m, 0.45f * m, 9f, 0.45f * m, "panel"));
        // 4: emblem door in the back wall, 1.8 x 2.4 m, hazard-striped foot, red header
        float ex = u(0.475f);
        Box(f, "EmblemDoorFrame", ex, f.y + 1.2f * m + 0.1f, f.zb - 0.08f, 1.8f * m + 0.3f, 2.4f * m + 0.2f, 0.06f, "dark");
        Box(f, "EmblemDoor", ex, f.y + 1.2f * m, f.zb - 0.12f, 1.8f * m, 2.4f * m, 0.04f, "panel");
        var ea = Box(f, "EmblemA", ex - 0.4f, f.y + 1.4f * m, f.zb - 0.15f, 0.35f, 1.6f, 0.02f, "red"); ea.transform.rotation = Quaternion.Euler(0, 0, 35f);
        var eb = Box(f, "EmblemB", ex + 0.4f, f.y + 1.4f * m, f.zb - 0.155f, 0.35f, 1.6f, 0.02f, "red"); eb.transform.rotation = Quaternion.Euler(0, 0, -35f);
        for (int k = 0; k < 6; k++) Box(f, "EmblemHazard", ex - 1.1f + k * 0.45f, f.y + 0.2f, f.zb - 0.15f, 0.22f, 0.3f, 0.02f, k % 2 == 0 ? "red" : "dark");
        Box(f, "EmblemHeader", ex, f.y + 2.4f * m + 0.35f, f.zb - 0.1f, 1.8f * m + 0.3f, 0.3f, 0.08f, "lit");
        // 6: wall cabinet at chest height
        filmStructures.Add(Box(f, "WallCabinet", u(0.60f), f.y + 0.7f * m, f.zb - 0.25f * m - 0.05f, 0.7f * m, 1.4f * m, 0.5f * m, "panel"));
        // 7: two red spotlight cones from the ceiling, over the lockers and over the door; shoot the lamp, the cone goes
        int li = 0;
        foreach (var a in new[] { 0.25f, 0.50f })
        {
            float x = u(a), lz = f.zb - 1.2f * m;
            Box(f, "Lamp", x, f.y + 6.3f, lz, 1.0f, 0.3f, 0.6f, "dark");
            var glow = Box(f, "LampGlow", x, f.y + 6.13f, lz, 0.8f, 0.04f, 0.4f, "screen");
            var dead = Box(f, "LampDead", x, f.y + 6.125f, lz, 0.78f, 0.04f, 0.38f, "black");
            var beam = new GameObject("Beam");
            beam.transform.SetParent(f.Parent, false);
            beam.transform.position = new Vector3(x, f.y, lz);
            beam.transform.localScale = new Vector3(1.8f * m, 6.1f, 1.8f * m); // zzh: a round volume, not a card
            beam.AddComponent<MeshFilter>().sharedMesh = ConeMesh();
            var r = beam.AddComponent<MeshRenderer>();
            r.sharedMaterial = f.M["beam"];
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
            Breakable(f, "Spot" + li++, new Vector3(x, f.y + 6.2f, lz), new Vector3(1.0f, 0.4f, 0.8f),
                new List<GameObject> { glow, beam }, new List<GameObject> { dead }, LampHits, "", LampBreakSound);
        }
        // 8-9: free-standing cabinet and the lower crate beside it, the right half's cover
        filmStructures.Add(Box(f, "FreeCabinet", u(0.64f), f.y + 0.8f * m, f.zb - 1.5f * m, 1.4f * m, 1.6f * m, 1.0f * m, "panel"));
        Box(f, "FreeCabinetSeam", u(0.64f), f.y + 0.8f * m, f.zb - 2.0f * m - 0.03f, 0.05f, 1.5f * m, 0.02f, "dark");
        filmStructures.Add(Box(f, "LowCrate", u(0.53f), f.y + 0.45f * m, f.zb - 2.0f * m, 1.1f * m, 0.9f * m, 1.0f * m, "panel"));
        Box(f, "LowCrateStrap", u(0.53f), f.y + 0.45f * m, f.zb - 2.0f * m, 0.18f, 0.9f * m + 0.02f, 1.0f * m + 0.02f, "red");
        // 10: wall box on a vertical pipe, a hose loop under it
        float wx = u(0.83f);
        Box(f, "WallPipe", wx, f.y + 4.5f, f.zb - 0.12f, 0.15f, 9f, 0.15f, "steel");
        Box(f, "WallBox", wx, f.y + 1.9f * m, f.zb - 0.25f, 0.7f * m, 0.55f * m, 0.3f, "dark");
        var hose = Box(f, "Hose", wx + 0.3f, f.y + 1.3f * m, f.zb - 0.2f, 0.08f, 1.0f, 0.08f, "black"); hose.transform.rotation = Quaternion.Euler(0, 0, 20f);
        // 11: steps with a landing at the right (u 0.77, not the film 0.88, to stay 3.5 units clear of the right-wall door), rise 0.45 m, 1.4 m wide: one solid stepped block, not a route
        float sx = u(0.77f), sw = 1.4f * m, rise = 0.45f * m, tread = 0.45f * m;
        var landing = Box(f, "StepLanding", sx, f.y + rise / 2, f.zb - 0.6f * m, sw, rise, 1.2f * m, "panel");
        filmStructures.Add(landing);
        // the treads are under Solidify's 0.8 height floor, so one collider covers landing and treads (solid, not a route)
        float run = 1.2f * m + 4f * tread;
        var sc = landing.GetComponent<BoxCollider>(); if (sc == null) sc = landing.AddComponent<BoxCollider>(); // never ?? on a Unity component (fake null)
        if (FilmRoom8Steps)
        {
            // zzj: the landing is a floor (layer Terrain, tag Floor), the treads are drawn only, an invisible ramp lies over them
            sc.size = Vector3.one; sc.center = Vector3.zero;
            landing.layer = TerrainLayer; landing.tag = "Floor"; solidCount++;
            WalkRamp(f, "Steps", "room 8 steps", new Vector3(sx, 0f, f.zb - 1.2f * m), Vector3.back, 4f * tread, rise, sw);
            StepsDoorway(f, f.x1 - 2.0f); // right of the wall pipe, clear of its box and the corner post
        }
        else
        {
            sc.size = new Vector3(1f, 1f, run / (1.2f * m)); sc.center = new Vector3(0f, 0f, -(run - 1.2f * m) / 2f / (1.2f * m));
            landing.layer = ObstacleLayer; solidCount++;
        }
        Box(f, "StepLandingEdge", sx, f.y + rise + 0.02f, f.zb - 1.2f * m + 0.06f, sw + 0.02f, 0.04f, 0.12f, "red");
        for (int k = 1; k <= 4; k++)
        {
            float h = rise * (1f - k / 5f), z = f.zb - 1.2f * m - (k - 0.5f) * tread;
            filmStructures.Add(Box(f, "Step", sx, f.y + h / 2, z, sw, h, tread, k % 2 == 0 ? "panel" : "steel"));
            Box(f, "StepNose", sx, f.y + h + 0.01f, z - tread / 2 + 0.04f, sw + 0.02f, 0.03f, 0.1f, k % 2 == 0 ? "red" : "white");
        }
    }

    // Build zzj (owner, after playing zzh: "no steps next to a door"): the film's steps stand right beside a dark back
    // doorway under a hood (frame t495 = 8:15 in the lab's film-geometry/height-zzi/). The steps become walkable by room
    // 4's recipe: the landing (0.675 units) is a floor on layer Terrain with tag Floor, the four treads are drawn only,
    // and an invisible Interactive ramp over them (2.7 units, 14 degrees) carries characters up, waypoints at foot and
    // landing. The doorway is drawn on the back wall right of the landing (dark opening, frame, hood): backdrop, not a
    // door, so the route and the right-wall door are untouched. Set false to build zzi's solid stepped block.
    const bool FilmRoom8Steps = true;
    static void StepsDoorway(Frame f, float x)
    {
        float m = 1.5f, w = 1.4f * m, h = 2.3f * m;
        Box(f, "BackDoorFrame", x, f.y + h / 2 + 0.08f, f.zb - 0.06f, w + 0.3f, h + 0.16f, 0.06f, "dark");
        Box(f, "BackDoorOpening", x, f.y + h / 2, f.zb - 0.1f, w, h, 0.03f, "black");
        Box(f, "BackDoorHood", x, f.y + h + 0.3f, f.zb - 0.45f, w + 0.3f, 0.18f, 0.9f, "steel");
        Box(f, "BackDoorHoodEdge", x, f.y + h + 0.3f, f.zb - 0.92f, w + 0.32f, 0.2f, 0.04f, "red");
        Box(f, "BackDoorSill", x, f.y + 0.02f, f.zb - 0.4f, w, 0.04f, 0.6f, "red");
    }

    // 9: break room: serving counter under a row of glowing menu panels, tables, a drum.
    static void BreakRoom(Frame f)
    {
        if (FilmRoom9Structures) { FilmCanteen(f); return; }
        float cx = f.Cx + 2f;
        Box(f, "ServeCounter", cx, f.y + 0.7f, f.zb - 1.0f, 10f, 1.4f, 1.4f, "panel"); // round 5 judge kept the zn box here
        Box(f, "ServeTop", cx, f.y + 1.45f, f.zb - 1.0f, 10.3f, 0.1f, 1.6f, "dark");
        Box(f, "ServeStripe", cx, f.y + 1.0f, f.zb - 1.72f, 10f, 0.25f, 0.02f, "red");
        for (int i = 0; i < 3; i++) Box(f, "MenuPanel", cx - 3.4f + i * 3.4f, f.y + 4.1f, f.zb - 0.1f, 2.8f, 1.5f, 0.06f, "screen");
        for (int i = 0; i < 3; i++) Box(f, "MenuFrame", cx - 3.4f + i * 3.4f, f.y + 4.1f, f.zb - 0.06f, 3.0f, 1.7f, 0.04f, "dark");
        Box(f, "Table", f.x0 + 6f, f.y + 0.9f, f.zb - 4f, 2.6f, 0.12f, 1.4f, "dark");
        Box(f, "TableLeg", f.x0 + 6f, f.y + 0.45f, f.zb - 4f, 0.2f, 0.9f, 0.2f, "dark");
        Cyl(f, "Drum", f.x0 + 2.5f, f.y + 0.8f, f.zb - 0.9f, 1.2f, 1.6f, "steel");
        Box(f, "DrumBand", f.x0 + 2.5f, f.y + 1.1f, f.zb - 0.9f, 1.25f, 0.15f, 1.25f, "red");
        // a slate board with chalk lines, a fridge and a vending machine, a banner
        Box(f, "Slate", f.x0 + 4.5f, f.y + 3.6f, f.zb - 0.1f, 3.2f, 2.0f, 0.06f, "dark");
        for (int i = 0; i < 4; i++) Box(f, "Chalk", f.x0 + 4.5f - 0.2f * (i % 2), f.y + 4.2f - i * 0.4f, f.zb - 0.14f, 2.2f - 0.3f * i, 0.06f, 0.02f, "white");
        Model2(Box(f, "Fridge", f.x1 - 2.2f, f.y + 2.0f, f.zb - 0.7f, 1.6f, 4.0f, 1.2f, "white"), "nx_fridge");
        Box(f, "FridgeLine", f.x1 - 2.2f, f.y + 2.7f, f.zb - (MoreModelledProps ? 1.38f : 1.31f), 1.6f, 0.05f, 0.02f, "dark");
        Model2(Box(f, "Vending", f.x1 - 4.2f, f.y + 1.9f, f.zb - 0.7f, 1.6f, 3.8f, 1.2f, "dark"), "nx_vending");
        Box(f, "VendingFace", f.x1 - 4.2f, f.y + 2.3f, f.zb - 1.32f, 1.2f, 2.4f, 0.03f, "screen");
        Banner(f, f.x0 + 1.9f, f.y + 3.4f, f.zb - 0.1f, false);
    }

    // ---- Film geometry, room 9 (build zzg): the film's canteen, set A, 10:14-10:33.5 (structure sheet
    // ai-docs/notes/film-geometry/room-09-break-room.md). 1 m = 1.5 units. Solid on the one floor: the serving
    // counter (4.5 m long, 1.0 m tall, 1.0 m deep, 0.2 m off the wall, u 0.62-0.84 on its red plinth) is the room's
    // firing post; the wall jog right of the back doorway (0.4 m proud); the taped carton out on the floor (low cover).
    // Backdrop: the serving hatch over the counter with the menu board, the round red light, the back doorway's deep
    // red-jambed frame with a closed panel, the bin. Left out: the raised walkway along the back wall (a route: the
    // chain's one walkable proof, room 4's deck, waits on its play before a second, prompt C1), the corridor mouth
    // at u 0.05-0.22 and the doorway as an opening (fake doors off), the right-wall door (doors unchanged), set B's
    // barrels, table and posters. Removed under the toggle: centre counter, menu panels, table, drum, slate, fridge,
    // vending machine, banner (not in set A). Set false to build zzf's break room.
    const bool FilmRoom9Structures = true;
    static void FilmCanteen(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f, zb = f.zb;
        // A4: serving counter, top 1.0 m, 1.0 m deep, 0.2 m off the wall; ends 3.5 units clear of the right-wall door
        float c0 = u(0.62f), c1 = c0 + 4.5f * m, ch = 1.0f * m, cd = 1.0f * m, cz = zb - 0.2f * m - cd / 2;
        var counter = Box(f, "ServeCounter", (c0 + c1) / 2, f.y + ch / 2, cz, c1 - c0, ch, cd, "panel");
        if (counter.GetComponent<BoxCollider>() == null) { counter.AddComponent<BoxCollider>(); counter.layer = ObstacleLayer; solidCount++; }
        filmStructures.Add(counter);
        Box(f, "ServeTop", (c0 + c1) / 2, f.y + ch + 0.05f, cz, c1 - c0 + 0.2f, 0.1f, cd + 0.15f, "dark");
        Box(f, "ServePlinth", (c0 + c1) / 2, f.y + 0.25f, zb - 0.2f * m - cd - 0.02f, c1 - c0, 0.5f, 0.02f, "red");
        for (int k = 0; k < 3; k++) Cyl(f, "Cup", c0 + 1.5f + k * 1.6f, f.y + ch + 0.25f, cz, 0.25f, 0.35f, k == 1 ? "red" : "white");
        // A5: serving hatch u 0.62-0.95 (3.6 x 1.6 m) over the counter, menu board on its right; A6: round red light at u 0.80
        float hx = (c0 + c1) / 2, hw = 3.6f * m * 0.8f, hy = f.y + ch + 0.6f + 0.8f * m;
        Box(f, "HatchOpening", hx - 0.6f, hy, zb - 0.03f, hw, 1.6f * m, 0.02f, "black");
        Box(f, "HatchFrameTop", hx - 0.6f, hy + 0.8f * m + 0.1f, zb - 0.1f, hw + 0.3f, 0.2f, 0.12f, "dark");
        Box(f, "HatchFrameFoot", hx - 0.6f, hy - 0.8f * m - 0.1f, zb - 0.1f, hw + 0.3f, 0.2f, 0.12f, "dark");
        Box(f, "MenuPanel", c1 + 0.2f, hy, zb - 0.1f, 1.6f, 2.0f, 0.06f, "screen");
        Cyl(f, "RedLight", u(0.80f), hy + 0.8f * m + 1.0f, zb - 0.1f, 0.7f * m, 0.12f, "screen");
        // A2: back doorway u 0.30-0.48 in a deep frame with red jambs, 0.4 m proud, shown closed (fake doors off)
        float dx = u(0.39f), dw = 1.6f * m, dh = 2.4f * m, pr = 0.4f * m;
        foreach (var sx in new[] { -1, 1 }) Box(f, "DoorJamb", dx + sx * (dw / 2 + 0.15f), f.y + dh / 2 + 0.05f, zb - pr / 2, 0.3f, dh, pr, "red");
        Box(f, "DoorHeaderBlock", dx, f.y + dh + 0.35f, zb - pr / 2, dw + 0.6f, 0.5f, pr, "wall");
        Box(f, "DoorPanelClosed", dx, f.y + dh / 2 + 0.05f, zb - 0.06f, dw, dh, 0.04f, "dark");
        // A7: wall jog right of the doorway, u 0.48-0.60 (1.1 m wide), 0.4 m proud, full height, the band bending round it
        float jx = u(0.48f) + 0.55f * m + 0.3f;
        filmStructures.Add(Box(f, "WallJog", jx, f.y + 4.5f, zb - pr / 2, 1.1f * m, 9f, pr, "wall"));
        Box(f, "WallJogBand", jx, f.y + 0.55f * 2.7f, zb - pr / 2, 1.1f * m + 0.06f, 0.45f * m, pr + 0.06f, "red");
        // A10: taped carton out on the near left floor (low cover), a bin by the doorway
        float tx = u(CartonU), tz = zb - CartonOut * m, ts = 1.1f * m;
        filmStructures.Add(Box(f, "Carton", tx, f.y + ts / 2, tz, ts, ts, ts * 0.8f, "panel"));
        Box(f, "CartonTape", tx, f.y + ts / 2, tz, 0.2f, ts + 0.02f, ts * 0.8f + 0.02f, "steel");
        Cyl(f, "Bin", u(0.55f) - 1.6f, f.y + 0.45f * m, zb - 0.8f * m - 0.8f, 0.6f * m, 0.9f * m, "steel");
    }
    const float CartonU = 0.15f, CartonOut = 5f;

    // 10: control tower: a wall of red monitors above a long desk, server racks at the sides.
    static void ControlTower(Frame f)
    {
        if (FilmRoom10Structures) { AdminRoom(f); return; }
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 6; c++)
            {
                float x = f.Cx - 6.25f + c * 2.5f, yy = f.y + 2.6f + r * 1.5f;
                Box(f, "MonitorCase", x, yy, f.zb - 0.2f, 2.3f, 1.35f, 0.25f, "black");
                var screen = Box(f, "MonitorScreen", x, yy, f.zb - 0.34f, 2.0f, 1.1f, 0.02f, (r + c) % 4 == 1 ? "dark" : "screen");
                // shot screen: black glass with a crack across it
                var off = Box(f, "MonitorDead", x, yy, f.zb - 0.34f, 2.0f, 1.1f, 0.02f, "black");
                var crack = Box(f, "MonitorCrack", x + 0.1f, yy, f.zb - 0.36f, 1.6f, 0.04f, 0.01f, "steel");
                crack.transform.rotation = Quaternion.Euler(0, 0, 18f + c * 11f);
                Breakable(f, "Monitor" + r + c, new Vector3(x, yy, f.zb - 0.3f), new Vector3(2.0f, 1.1f, 0.2f),
                    new List<GameObject> { screen }, new List<GameObject> { off, crack }, MonitorHits, MonitorHitSound, MonitorBreakSound, MonitorExplosion);
            }
        Model(Box(f, "Desk", f.Cx, f.y + 0.75f, f.zb - 1.6f, 15f, 1.5f, 1.4f, "panel"), "nx_desk");
        Box(f, "DeskTop", f.Cx, f.y + 1.55f, f.zb - 1.6f, 15.4f, 0.1f, 1.6f, "dark");
        for (int i = 0; i < 4; i++) Box(f, "Keyboard", f.Cx - 5f + i * 3.3f, f.y + 1.63f, f.zb - 1.9f, 1.4f, 0.06f, 0.5f, "black");
        foreach (var x in new[] { f.x0 + 2.5f, f.x0 + 4f, f.x1 - 4f, f.x1 - 2.5f })
        {
            Box(f, "Rack", x, f.y + 2.0f, f.zb - 0.8f, 1.3f, 4.0f, 1.2f, "black");
            for (float yy = f.y + 0.6f; yy < f.y + 3.8f; yy += 0.45f) Box(f, "RackLed", x - 0.35f, yy, f.zb - 1.42f, 0.25f, 0.06f, 0.02f, "screen");
        }
        // the top of the tower: a red sky strip above the monitor wall, banners flanking it, a door to the server room
        Box(f, "SkyStrip", f.Cx, f.y + 6.6f, f.zb - 0.05f, f.W - 2f, 0.9f, 0.04f, "sky");
        Box(f, "SkyStripFrame", f.Cx, f.y + 6.1f, f.zb - 0.12f, f.W - 2f, 0.12f, 0.12f, "dark");
        Banner(f, f.Cx - 9f, f.y + 3.6f, f.zb - 0.1f, false);
        Banner(f, f.Cx + 9f, f.y + 3.6f, f.zb - 0.1f, false);
        BackDoorway(f, f.x1 - 6.5f);
    }

    // ---- Film geometry, room 10 (build zy): the film's admin room, 10:34-11:01 (structure sheet
    // ai-docs/notes/film-geometry/room-10-control-tower.md, owner frame 10:47). 1 m = 1.5 units, as in zw.
    // The map room is wider than the film's 15 m, so pieces keep the film's left-to-right order at film size
    // and the extra floor stays bare (sheet note). Solid on the one floor: the hooded console (u 0.285), the
    // narrow cabinet beside it, the low bench (u 0.57) and the tower unit (u 0.74). Backdrop: the recessed
    // panel with its diagonal trim, the left pilaster, the two screens hung from ceiling mounts (breakable,
    // as zw's monitors were), the tall red light panel, a floor mat under the bench, a lit sign over the
    // right-wall door. The monitor wall, desk, racks, sky strip and banners are not in the film set.
    // Set false to build zx's control tower.
    const bool FilmRoom10Structures = true;
    static void AdminRoom(Frame f)
    {
        float u(float a) => f.x0 + a * f.W;
        float m = 1.5f;
        // 1: recessed back-wall panel, 3.6 x 3.4 m, with a stepped diagonal red trim on its left edge
        float rx = u(0.20f);
        Box(f, "RecessPanel", rx, f.y + 1.7f * m + 0.6f, f.zb - 0.06f, 3.6f * m, 3.4f * m, 0.04f, "dark");
        var trim = Box(f, "RecessTrim", rx - 1.4f * m, f.y + 1.7f * m + 0.6f, f.zb - 0.1f, 0.18f, 3.0f * m, 0.03f, "red");
        trim.transform.rotation = Quaternion.Euler(0f, 0f, -20f);
        // 2: left pilaster at the back corner, full height
        Box(f, "Pilaster", f.x0 + 0.25f, f.y + 4.5f, f.zb - 0.4f, 0.5f, 9f, 0.75f, "panel");
        // 3: hooded console against the back wall, 1.8 x 2.5 x 0.8 m: body, sloped hood, red screen, box on top
        float cx = u(0.285f), cd = 0.8f * m, cz = f.zb - cd / 2 - 0.05f;
        filmStructures.Add(Box(f, "AdminConsole", cx, f.y + 1.0f * m, cz, 1.8f * m, 2.0f * m, cd, "panel"));
        Wedge(f, "ConsoleHood", cx, f.y + 2.0f * m, cz, 1.8f * m, 0.5f * m, cd, 0.35f, "dark");
        Box(f, "ConsoleScreen", cx, f.y + 1.45f * m, f.zb - cd - 0.07f, 1.3f * m, 0.6f * m, 0.03f, "screen");
        Box(f, "ConsoleTopBox", cx + 0.5f * m, f.y + 2.5f * m + 0.2f, cz + 0.2f, 0.4f * m, 0.3f * m, 0.3f * m, "black");
        // 4: narrow cabinet right of it
        filmStructures.Add(Box(f, "NarrowCabinet", cx + 0.9f * m + 0.3f, f.y + 0.9f * m, f.zb - 0.3f * m - 0.05f, 0.3f * m, 1.8f * m, 0.6f * m, "steel"));
        // 5: two screens hung from ceiling mounts, 1.4 x 1.0 m, bottom edge at 2.2 m; they break as zw's monitors did
        foreach (var (a, face) in new[] { (0.45f, "screen"), (0.615f, "lit") })
        {
            float sx = u(a), sy = f.y + 2.2f * m + 0.5f * m, sz = f.zb - 0.3f * m - 0.1f;
            Box(f, "ScreenMount", sx, sy + 0.5f * m + 0.45f * m + 0.15f, sz, 0.12f, 0.9f * m, 0.12f, "dark");
            var cas = Box(f, "HungScreenCase", sx, sy, sz, 1.4f * m + 0.15f, 1.0f * m + 0.15f, 0.2f, "black");
            var scr = Box(f, "HungScreen", sx, sy, sz - 0.12f, 1.4f * m, 1.0f * m, 0.02f, face);
            var dead = Box(f, "HungScreenDead", sx, sy, sz - 0.115f, 1.4f * m - 0.02f, 1.0f * m - 0.02f, 0.02f, "black");
            var crack = Box(f, "HungScreenCrack", sx, sy, sz - 0.14f, 1.6f, 0.04f, 0.01f, "steel");
            crack.transform.rotation = Quaternion.Euler(0f, 0f, 25f); // hung flat, not tilted: a tilt crossed the case (overlap scan)
            Breakable(f, "HungScreen" + a, new Vector3(sx, sy, sz - 0.1f), new Vector3(1.4f * m, 1.0f * m, 0.2f),
                new List<GameObject> { scr }, new List<GameObject> { dead, crack }, MonitorHits, MonitorHitSound, MonitorBreakSound, MonitorExplosion);
        }
        // 6: tall red light panel, 0.65 x 3.1 m, bottom at 1.0 m
        float lx = u(0.735f), ly = f.y + 1.0f * m + 1.55f * m;
        Box(f, "RedPanelFrame", lx, ly, f.zb - 0.09f, 0.65f * m + 0.3f, 3.1f * m + 0.3f, 0.08f, "dark");
        Box(f, "RedPanel", lx, ly, f.zb - 0.14f, 0.65f * m, 3.1f * m, 0.03f, "screen");
        // 7: low bench, 2.9 x 0.7 x 0.6 m, on a floor mat (a mat, not a step: the sheet's reading)
        float bx = u(0.57f), bz = f.zb - 0.6f * m - 0.2f;
        Box(f, "BenchMat", u(0.67f), f.y + 0.0015f, f.zb - 1.65f * m, 7.8f * m, 0.003f, 2.7f * m, "dark");
        filmStructures.Add(Box(f, "AdminBench", bx, f.y + 0.35f * m, bz, 2.9f * m, 0.7f * m, 0.6f * m, "panel"));
        Box(f, "AdminBenchLid", bx, f.y + 0.7f * m + 0.04f, bz, 2.9f * m + 0.08f, 0.08f, 0.6f * m + 0.08f, "dark");
        // 8: tower unit right of the bench
        filmStructures.Add(Box(f, "TowerUnit", u(0.70f), f.y + 0.45f * m, f.zb - 0.4f * m - 0.2f, 0.75f * m, 0.9f * m, 0.5f * m, "black"));
        // 9: lit sign over the right-wall door (the late pair's way in)
        var rd = f.Doors.Where(d => d.x > f.x1 - 3f).OrderByDescending(d => d.z).ToList();
        if (rd.Count > 0) Box(f, "AdminSign", f.x1 - 0.12f, f.y + 6.2f, rd[0].z, 0.06f, 0.35f * m, 1.1f * m, "lit");
        // 13: bin by the right wall, a knocked wet-floor sign mid-floor
        Cyl(f, "Bin", u(0.88f), f.y + 0.4f * m, f.zb - 2.5f * m, 0.5f * m, 0.75f * m, "steel");
        var wet = Box(f, "WetSign", u(0.56f), f.y + 0.06f, f.zb - 2.5f * m, 0.5f * m, 0.08f, 0.8f * m, "white");
        wet.transform.rotation = Quaternion.Euler(0f, 30f, 0f);
        BackDoorway(f, f.x1 - 6.5f);
    }

    // Faction-style banner: red board, dark border, a plain chevron (an original mark, not the game's logo).
    static void Banner(Frame f, float x, float y, float z, bool onSide)
    {
        float w = 1.8f, h = 2.6f;
        if (onSide)
        {
            float inward = x < f.Cx ? 1 : -1;
            Box(f, "BannerBorder", x, y, z, 0.05f, h + 0.2f, w + 0.2f, "dark");
            Box(f, "Banner", x + inward * 0.04f, y, z, 0.04f, h, w, "red");
            var a = Box(f, "BannerMarkA", x + inward * 0.07f, y + 0.1f, z - 0.3f, 0.02f, 1.2f, 0.25f, "dark");
            a.transform.rotation = Quaternion.Euler(35f, 0, 0);
            var b = Box(f, "BannerMarkB", x + inward * 0.075f, y + 0.1f, z + 0.3f, 0.02f, 1.2f, 0.25f, "dark");
            b.transform.rotation = Quaternion.Euler(-35f, 0, 0);
        }
        else
        {
            Box(f, "BannerBorder", x, y, z, w + 0.2f, h + 0.2f, 0.05f, "dark");
            Box(f, "Banner", x, y, z - 0.04f, w, h, 0.04f, "red");
            var a = Box(f, "BannerMarkA", x - 0.3f, y + 0.1f, z - 0.07f, 0.25f, 1.2f, 0.02f, "dark");
            a.transform.rotation = Quaternion.Euler(0, 0, 35f);
            var b = Box(f, "BannerMarkB", x + 0.3f, y + 0.1f, z - 0.075f, 0.25f, 1.2f, 0.02f, "dark");
            b.transform.rotation = Quaternion.Euler(0, 0, -35f);
        }
    }

    // Recessed doorway in the back wall with the video's red-and-dark hazard header.
    // Off since 2026-11-11 (Workshop feedback: "doors on fake doors"): a painted doorway beside real exits
    // reads as a door the player cannot use. Real doors keep their frames and hazard bars from Shell.
    const bool FakeDoors = false;
    static void BackDoorway(Frame f, float x)
    {
        if (!FakeDoors) return;
        Box(f, "BackDoorFrame", x, f.y + 2.4f, f.zb - 0.12f, 3.0f, 4.8f, 0.1f, "dark");
        Box(f, "BackDoor", x, f.y + 2.3f, f.zb - 0.18f, 2.5f, 4.6f, 0.04f, "black");
        for (int k = 0; k < 5; k++)
            Box(f, "BackHazard", x - 1.2f + k * 0.6f, f.y + 5.2f, f.zb - 0.12f, 0.3f, 0.4f, 0.06f, k % 2 == 0 ? "red" : "dark");
    }

    static void Crate(Frame f, float x, float z, float size, float stackOn = 0f)
    {
        if (f.Doors.Any(d => Mathf.Abs(d.x - x) < 1.5f + size / 2 && Mathf.Abs(d.z - z) < 1.5f + size / 2)) return;
        float cy = f.y + stackOn + size / 2;
        var body = Box(f, "Crate", x, cy, z, size, size, size, "panel");
        HeroCrateMesh(body, size, cy);
        Box(f, "CrateStrapA", x - size * 0.25f, cy, z, 0.18f, size + 0.02f, size + 0.02f, "red");
        Box(f, "CrateStrapB", x + size * 0.25f, cy, z, 0.18f, size + 0.02f, size + 0.02f, "red");
        Box(f, "CrateLid", x, cy + size / 2, z, size + 0.06f, 0.08f, size + 0.06f, "dark");
    }

    // Round 3 (build zm): the first modelled prop. A crate made in code (polywright code route; lab prop-zm,
    // 1.0 x 1.2 x 1.0 m, base at y 0, Y up, metres) imported as OBJ into MatDir/Models swaps the crate body's
    // cube; the collider stays the box. Logs the imported size so the import scale is proved, not assumed.
    // false = the zl box crate.
    const bool HeroCrate = true;
    static Mesh heroCrate; static bool heroLogged;
    static void HeroCrateMesh(GameObject body, float size, float cy)
    {
        if (!HeroCrate || body == null) return;
        if (heroCrate == null) heroCrate = AssetDatabase.LoadAllAssetsAtPath(MatDir + "/Models/nx_crate.obj").OfType<Mesh>().FirstOrDefault();
        if (heroCrate == null) { Debug.LogWarning("NexusProminence: " + MatDir + "/Models/nx_crate.obj missing; box crate kept"); return; }
        var b = heroCrate.bounds;
        if (!heroLogged) { heroLogged = true; Debug.Log("NexusProminence: hero crate imported size " + b.size.x.ToString("0.000") + " x " + b.size.y.ToString("0.000") + " x " + b.size.z.ToString("0.000") + " (file 1.000 x 1.200 x 1.000 m), min y " + b.min.y.ToString("0.000")); }
        var col = body.GetComponent<BoxCollider>(); var cs = col != null ? col.size : Vector3.one; var cc = col != null ? col.center : Vector3.zero;
        body.GetComponent<MeshFilter>().sharedMesh = heroCrate;
        var t = body.transform;
        t.localScale = new Vector3(size / b.size.x, size / b.size.y, size / b.size.z);
        t.position = new Vector3(t.position.x, cy - size / 2 - b.min.y * t.localScale.y, t.position.z);
        if (col != null) { col.size = new Vector3(cs.x * b.size.x, cs.y * b.size.y, cs.z * b.size.z); col.center = b.center; }
    }

    // Round 5 (build zo, concern 5 care and appeal): the most visible box props become modelled props made in
    // code (tools/make_props.py, lab prop-zo, metres, Y up, centred, at the size of the box they replace), on the
    // round 3 path: OBJ imported 1:1 into MatDir/Models. The box keeps its place, material and collider size, so
    // cover and pathing are unchanged. Logs each imported size once. false = the zn boxes.
    const bool ModelledProps = true;
    static readonly Dictionary<string, Mesh> propMeshes = new Dictionary<string, Mesh>();
    static GameObject Model(GameObject s, string obj)
    {
        if (!ModelledProps || s == null) return s;
        Mesh m;
        if (!propMeshes.TryGetValue(obj, out m))
        {
            m = AssetDatabase.LoadAllAssetsAtPath(MatDir + "/Models/" + obj + ".obj").OfType<Mesh>().FirstOrDefault();
            propMeshes[obj] = m;
            if (m == null) Debug.LogWarning("NexusProminence: " + MatDir + "/Models/" + obj + ".obj missing; box kept");
            else Debug.Log("NexusProminence: prop " + obj + " imported size " + m.bounds.size.x.ToString("0.000") + " x " + m.bounds.size.y.ToString("0.000") + " x " + m.bounds.size.z.ToString("0.000"));
        }
        if (m == null) return s;
        var b = m.bounds; var t = s.transform; var size = t.localScale;
        s.GetComponent<MeshFilter>().sharedMesh = m;
        t.localScale = new Vector3(size.x / b.size.x, size.y / b.size.y, size.z / b.size.z);
        t.position -= Vector3.Scale(b.center, t.localScale);
        var col = s.GetComponent<BoxCollider>();
        if (col != null) { col.size = b.size; col.center = b.center; }
        modelCount++;
        return s;
    }
    static int modelCount;

    // Round 7 (build zq, concern 3 second pass): the next most visible boxes the film shows as framed props, on the
    // same path: garage console, the two vending machines, the break room fridge (tools/make_props.py). Locker
    // frames stand 5 cm proud and bench slats have 9 cm gaps (both from round 5's rough edges). false = zp boxes
    // for the new props (the locker and bench OBJ changes ride on ModelledProps).
    const bool MoreModelledProps = true;
    static GameObject Model2(GameObject s, string obj) { return MoreModelledProps ? Model(s, obj) : s; }
    // Round 7: the film's large red fields drawn closer to their size where the frames show them (office back window).
    const bool FilmRedFields = true;

    static void OpenCrate(Frame f, float x, float z, float s)
    {
        float h = s / 2;
        foreach (var dx in new[] { -h, h })
            foreach (var dz in new[] { -h, h })
                Box(f, "Edge", x + dx, f.y + h, z + dz, 0.15f, s, 0.15f, "dark");
        foreach (var dy in new[] { 0f, s })
        {
            Box(f, "Edge", x, f.y + dy, z - h, s, 0.14f, 0.14f, "dark");
            Box(f, "Edge", x, f.y + dy, z + h, s, 0.14f, 0.14f, "dark");
            Box(f, "Edge", x - h, f.y + dy, z, 0.13f, 0.13f, s, "dark");
            Box(f, "Edge", x + h, f.y + dy, z, 0.13f, 0.13f, s, "dark");
        }
        var brace = Box(f, "Brace", x, f.y + h, z - h, s * 1.35f, 0.12f, 0.12f, "dark");
        brace.transform.rotation = Quaternion.Euler(0, 0, 45);
    }

    // ---- juice: set pieces from the film that break when shot (CustomObj_Destructible, converted by the
    // game into Interactive_Target). Feedback only: bullets pass through, no explosion unless a card is set,
    // no damage bar, no popup, so fights play exactly as before. Set JuiceBreakables false to build without.
    const bool JuiceBreakables = true;
    const int GlassHits = 1, LampHits = 2, MonitorHits = 2;
    const string GlassBreakSound = "event:/Foley/Glass Bottles/Wine Bottle Break";
    const string LampBreakSound = "event:/Foley/Glass Bottles/Beer Bottle Break";
    const string MonitorHitSound = "event:/Combat/Richochet/Richochet Sheild";
    const string MonitorBreakSound = "event:/MechanicalFoley/Sparks/Electrocution";
    const string MonitorExplosion = ""; // e.g. "ElecSplode" for a damaging spark burst (changes gameplay)

    // Wrap already-built visual pieces into a destructible: 'intact' shows until it breaks, 'broken' after.
    // The hit box is a thin collider at (center, size); keep it out of the walk lane.
    static void Breakable(Frame f, string name, Vector3 center, Vector3 size, List<GameObject> intact, List<GameObject> broken,
        int hits, string hitSound, string breakSound, string explosion = "")
    {
        if (!JuiceBreakables) { foreach (var g in broken) Object.DestroyImmediate(g); return; }
        var root = new GameObject("CO_NX_" + name);
        root.transform.SetParent(f.Parent, true);
        root.transform.position = center;
        var a = new GameObject("Intact"); a.transform.SetParent(root.transform, false);
        var b = new GameObject("Broken"); b.transform.SetParent(root.transform, false);
        foreach (var g in intact) { g.isStatic = false; g.transform.SetParent(a.transform, true); }
        foreach (var g in broken) { g.isStatic = false; g.transform.SetParent(b.transform, true); }
        b.SetActive(false);
        var hit = new GameObject("Hit"); hit.transform.SetParent(root.transform, false);
        var col = hit.AddComponent<BoxCollider>(); col.size = size;
        var d = root.AddComponent<CustomObj_Destructible>();
        d.AimTarget = hit;
        d.HitLocations = new List<Collider> { col };
        d.DestructionStates = new List<GameObject> { a, b };
        d.Health = hits; d.DamageIrrelevant = true;
        d.ShowDamageBar = false; d.FlashColorOnHit = true; d.AllBulletsPassThrough = true;
        d.HitSound = hitSound; d.DestroySound = breakSound; d.AdvanceStateSound = "";
        d.Explosion = explosion; d.ExplodePoint = hit.transform;
        d.Tasks = new[] { new CustomObj_Destructible.DestroyTask { PopupComplete = new string[0] } };
    }

    // Visual-only box (no collider, so pathing and the room's own collision are untouched).
    static GameObject Box(Frame f, string name, float x, float y, float z, float sx, float sy, float sz, string mat)
    {
        var s = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Object.DestroyImmediate(s.GetComponent<Collider>());
        s.name = "NX_" + name;
        s.transform.SetParent(f.Parent, true);
        s.transform.position = new Vector3(x, y, z);
        s.transform.localScale = new Vector3(sx, sy, sz);
        var r = s.GetComponent<MeshRenderer>();
        r.sharedMaterial = f.M[mat];
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        s.isStatic = true;
        Solidify(f, s, x, y - sy / 2, z, sx, sy, sz);
        return s;
    }

    // ---- overlap scan: wall filler (pilasters, conduits, clamps, stripes, seams) is laid on a fixed rhythm with
    // no idea where a room puts its doors, posters or cases, so it ran across doorways (owner's room 2 shot,
    // 2026-11-11). After a room is dressed: filler that crosses a feature or a real door opening is removed,
    // and features that cross each other are logged as NX_OVERLAP for the build log (fix those by hand).
    static readonly string[] Strips = { "Pipe_Back", "Skirt", "PanelRail", "SkyStripFrame" };
    static readonly string[] Filler = { "Pilaster", "PipeClamp", "Conduit", "BandStripe", "DiagStripe", "PanelSeam", "WallPanelStripes", "FloorSeam", "Grate", "Paper", "Stain", "OilStain" };
    static readonly string[] Features = { "Door", "Window", "LitWin", "Poster", "Case", "Sign", "Banner", "Board", "Slate", "Shutter", "Vending",
        "Notice", "Locker", "Menu", "Bill", "Vent", "Fridge", "Cabinet", "Rack", "Console", "Monitor", "Lamp", "Ledge", "Counter", "Pylon", "Pillar", "Barricade", "SlopeBlock", "Generator", "GasCan", "HazardSign", "Toolbox", "Hatch", "Crate", "Desk", "Table", "Bench", "Ladder", "BayPanel", "BayFrame" }; // zzh: filler never crosses room 2's bay (glitch shot 6)
    static string Family(string name, string[] keys)
    {
        string n = name.StartsWith("NX_") ? name.Substring(3) : name;
        foreach (var k in keys) if (n.StartsWith(k) || n.Contains(k)) return k;
        return null;
    }
    static void ClearWallOverlaps(Frame f, RoomObj room)
    {
        var rs = f.Parent.GetComponentsInChildren<Renderer>(true).Where(r => r.name.StartsWith("NX_")).ToList();
        var feats = new List<KeyValuePair<string, Bounds>>();
        foreach (var r in rs)
        {
            string fam = Family(r.name, Filler) == null ? Family(r.name, Features) : null;
            if (fam != null) feats.Add(new KeyValuePair<string, Bounds>(fam + ":" + r.name, r.bounds));
        }
        // the real door openings, as solid blocks, so filler never crosses an exit
        foreach (var d in f.Doors)
            feats.Add(new KeyValuePair<string, Bounds>("Door:opening", new Bounds(new Vector3(d.x, f.y + 2.2f, d.z), new Vector3(3.0f, 4.4f, 3.0f))));
        int removed = 0;
        // long strips (pipes, rails, skirting) are cut around what they cross instead of removed
        foreach (var r in rs.Where(r => Family(r.name, Strips) != null && r.transform.rotation == Quaternion.identity).ToList())
        {
            var b = r.bounds;
            int ax = b.size.x >= b.size.z ? 0 : 2;
            if (b.size[ax] < 4f) continue;
            var cuts = feats.Where(k => { var c = b; c.Expand(-0.02f); return k.Value.Intersects(c); })
                .Select(k => new Vector2(k.Value.min[ax] - 0.1f, k.Value.max[ax] + 0.1f)).OrderBy(c => c.x).ToList();
            if (cuts.Count == 0) continue;
            float a0 = b.min[ax];
            foreach (var c in cuts.Concat(new[] { new Vector2(b.max[ax] + 1f, b.max[ax] + 1f) }))
            {
                float a1 = Mathf.Min(c.x, b.max[ax]);
                if (a1 - a0 > 0.3f)
                {
                    var seg = Object.Instantiate(r.gameObject, r.transform.parent);
                    seg.name = r.name;
                    var p = r.transform.position; p[ax] = (a0 + a1) / 2; seg.transform.position = p;
                    var sc = r.transform.localScale; sc[ax] = sc[ax] * (a1 - a0) / b.size[ax]; seg.transform.localScale = sc;
                }
                a0 = Mathf.Max(a0, c.y);
            }
            Debug.Log("NX_CUT " + room.name + ": " + r.name + " around " + cuts.Count + " pieces");
            Object.DestroyImmediate(r.gameObject);
        }
        rs = f.Parent.GetComponentsInChildren<Renderer>(true).Where(r => r && r.name.StartsWith("NX_")).ToList();
        foreach (var r in rs)
        {
            if (Family(r.name, Filler) == null) continue;
            var b = r.bounds; b.Expand(-0.02f);
            var hit = feats.FirstOrDefault(k => k.Value.Intersects(b));
            // flat floor pieces only clash with what stands on the floor; wall filler with anything
            if (hit.Key == null) continue;
            if (b.size.y < 0.05f && hit.Value.min.y > f.y + 0.1f) continue;
            Debug.Log("NX_CLEAR " + room.name + ": " + r.name + " crossed " + hit.Key);
            Object.DestroyImmediate(r.gameObject);
            removed++;
        }
        for (int i = 0; i < feats.Count; i++)
            for (int j = i + 1; j < feats.Count; j++)
            {
                string fi = feats[i].Key.Split(':')[0], fj = feats[j].Key.Split(':')[0];
                if (fi == fj) continue;
                var a = feats[i].Value; a.Expand(-0.05f);
                if (a.Intersects(feats[j].Value)) Debug.Log("NX_OVERLAP " + room.name + ": " + feats[i].Key + " x " + feats[j].Key);
            }
        Debug.Log("NexusProminence: " + room.name + " overlap scan removed " + removed + " filler pieces");
        // dump what is left for polywright's checker: pw.py overlaps NexusBoxes/<room>.json
        var ci = System.Globalization.CultureInfo.InvariantCulture;
        System.Func<Vector3, string> v = p => "[" + p.x.ToString("0.###", ci) + "," + p.y.ToString("0.###", ci) + "," + p.z.ToString("0.###", ci) + "]";
        var rows = f.Parent.GetComponentsInChildren<Renderer>(true).Where(r => r && r.name.StartsWith("NX_")).Select(r =>
        {
            string fil = Family(r.name, Filler), feat = Family(r.name, Features);
            bool filler = fil != null || feat == null;
            return "{\"name\":\"" + r.name + "\",\"group\":\"" + (fil ?? feat ?? r.name) + "\",\"filler\":" + (filler ? "true" : "false")
                + ",\"min\":" + v(r.bounds.min) + ",\"max\":" + v(r.bounds.max) + "}";
        });
        string dir = System.IO.Path.Combine(Application.dataPath, "..", "NexusBoxes");
        System.IO.Directory.CreateDirectory(dir);
        System.IO.File.WriteAllText(System.IO.Path.Combine(dir, room.name + ".json"), "[" + string.Join(",", rows) + "]");
    }

    // ---- printed decals (textures from tools/make_decals.py in MatDir/Tex) ----
    static readonly Dictionary<string, Material> decalMats = new Dictionary<string, Material>();
    static Material DecalMat(string tex)
    {
        Material m;
        if (decalMats.TryGetValue(tex, out m) && m != null) return m;
        string tpath = MatDir + "/Tex/" + tex + ".png";
        var ti = AssetImporter.GetAtPath(tpath) as TextureImporter;
        if (ti == null) { Debug.LogWarning("NexusProminence: missing " + tpath + " (run tools/make_decals.py)"); return null; }
        if (!ti.alphaIsTransparency || ti.mipmapEnabled == false) { ti.alphaIsTransparency = true; ti.mipmapEnabled = true; ti.wrapMode = TextureWrapMode.Clamp; ti.SaveAndReimport(); }
        var t = AssetDatabase.LoadAssetAtPath<Texture2D>(tpath);
        bool cut = !tex.StartsWith("sign_") || tex == "sign_dontblink";
        string path = MatDir + "/NX_Decal_" + tex + ".mat";
        var sh = cut ? Shader.Find("Legacy Shaders/Transparent/Cutout/Diffuse") : Shader.Find("Standard");
        m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null) { m = new Material(sh); AssetDatabase.CreateAsset(m, path); }
        m.shader = sh; m.mainTexture = t; m.color = Color.white;
        if (cut) m.SetFloat("_Cutoff", 0.4f); else m.SetFloat("_Glossiness", 0.05f);
        EditorUtility.SetDirty(m);
        decalMats[tex] = m;
        return m;
    }

    // A quad on the back wall (faces the camera) or flat on the floor; w, h in units.
    static void Decal(Frame f, string tex, float x, float y, float z, float w, float h, bool floor = false, float spin = 0f)
    {
        var mat = DecalMat(tex); if (mat == null) return;
        var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Object.DestroyImmediate(q.GetComponent<Collider>());
        q.name = "NX_Decal_" + tex;
        q.transform.SetParent(f.Parent, true);
        q.transform.position = new Vector3(x, y, z);
        q.transform.rotation = floor ? Quaternion.Euler(90f, spin, 0f) : Quaternion.Euler(0f, 0f, spin);
        q.transform.localScale = new Vector3(w, h, 1f);
        var r = q.GetComponent<MeshRenderer>(); r.sharedMaterial = mat;
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        q.isStatic = true;
    }

    // zzh (owner's glitch shot 5, room 3): papers dropped at one height flickered over each other. Each paper takes
    // the next of six heights 0.004 apart (tops 0.022 to 0.042, clear of grates at 0.018), so two overlapping papers
    // never share a plane (mpn.py scene-lint S26 checks it).
    static int paperN;
    static float PaperY(Frame f) { return f.y + 0.02f + 0.004f * (paperN++ % 6); }
    static void Splat(Frame f, float u, float v, float size, int seed)
    {
        Decal(f, "splat_" + (seed % 4), Mathf.Lerp(f.x0, f.x1, u), f.y + 0.05f + (seed % 6) * 0.003f, Mathf.Lerp(f.zf, f.zb, v), size, size, true, seed * 47f);
    }
    const float SignScale = 1.9f;
    // Back-wall areas already taken by the set (doors, pillars, posters, windows, cabinets) and by earlier
    // decals, as x/y rectangles. Signs used to sit at fixed fractions and landed on doors and behind pillars.
    static List<Rect> wallBusy = new List<Rect>();
    static void CollectWallBusy(Frame f, RoomObj room)
    {
        wallBusy = new List<Rect>();
        var rs = f.Parent.GetComponentsInChildren<Renderer>(true).Concat(room.GetComponentsInChildren<ProxyObj_Door>(true).SelectMany(d => d.GetComponentsInChildren<Renderer>(true)));
        foreach (var r in rs)
        {
            var b = r.bounds;
            if (b.max.z < f.zb - 2.5f) continue;              // out on the floor, not against the wall
            if (b.size.x > f.W * 0.6f) continue;             // bands, pipes, skirting, sky strips span the wall
            if (b.size.y < 0.3f && b.size.x < 1f) continue;  // tiny trim
            if (b.size.x < 0.7f) continue;                   // pilasters, seams, rods, conduits, corner posts: signs may cover them
            if (r.name.Contains("Stripe") || r.name.Contains("Band") || r.name.Contains("Skirt") || r.name.Contains("Hazard") || r.name.Contains("Seam")) continue;
            if (b.max.y < f.y + 0.5f) continue;              // floor details
            wallBusy.Add(Rect.MinMaxRect(b.min.x, b.min.y, b.max.x, b.max.y));
        }
    }
    static bool WallFree(Rect r) { foreach (var b in wallBusy) if (b.Overlaps(r)) return false; return true; }

    static void WallDecal(Frame f, string tex, float u, float y, float w, float h, float spin = 0f)
    {
        w *= SignScale; h *= SignScale; // readable from the game camera
        y = Mathf.Min(y, 5.3f - h / 2f);
        bool soft = tex.StartsWith("holes_") || tex.StartsWith("splat_") || tex.StartsWith("scrawl_");
        float pad = 0.25f;
        // try the asked spot, then step outward along the wall, then a little higher or lower
        float w0 = w, h0 = h;
        foreach (var sc in new[] { 1f, 0.75f, 0.55f })
        foreach (var dy in new[] { 0f, 0.8f, -0.6f })
            for (int k = 0; k <= 30; k++)
            {
                w = w0 * sc; h = h0 * sc;
                float du = (k % 2 == 1 ? 1 : -1) * ((k + 1) / 2) * 0.04f;
                float uu = u + du;
                float x = Mathf.Lerp(f.x0, f.x1, uu);
                float yy = Mathf.Clamp(y + dy, h / 2f + 0.6f, 5.3f - h / 2f);
                if (x - w / 2 < f.x0 + 0.8f || x + w / 2 > f.x1 - 0.8f) continue;
                var r = new Rect(x - w / 2 - pad, f.y + yy - h / 2 - pad, w + 2 * pad, h + 2 * pad);
                if (!WallFree(r)) continue;
                Decal(f, tex, x, f.y + yy, f.zb - 0.47f, w, h, false, spin);
                wallBusy.Add(r);
                return;
            }
        Debug.Log("NexusProminence: no clear wall for " + tex + (soft ? " (dropped)" : " (sign dropped)"));
    }

    // ---- fun pass (2026-11-11): things to grab. Ammo and med cabinets on the back wall before the hard
    // rooms, and one floor weapon per room near the entrance, from the film's Nexus arsenal.
    const string InteractPrefab = "Assets/Library/Prefabs/Proxies/PX_Interact.prefab";
    const string ItemPrefab = "Assets/Library/Prefabs/Markers/Item_Spawn.prefab";
    static readonly ProxyObj_Interact.ProxyType[][] RoomStations =
    {
        new[] { ProxyObj_Interact.ProxyType.Ammo },
        new ProxyObj_Interact.ProxyType[0],
        new[] { ProxyObj_Interact.ProxyType.Ammo },
        new[] { ProxyObj_Interact.ProxyType.MedCabinet, ProxyObj_Interact.ProxyType.AmmoGrenade },
        new ProxyObj_Interact.ProxyType[0],
        new[] { ProxyObj_Interact.ProxyType.Ammo },
        new[] { ProxyObj_Interact.ProxyType.MedCabinet, ProxyObj_Interact.ProxyType.Ammo },
        new ProxyObj_Interact.ProxyType[0],
        new[] { ProxyObj_Interact.ProxyType.MedCabinet },
        new[] { ProxyObj_Interact.ProxyType.MedCabinet, ProxyObj_Interact.ProxyType.Ammo, ProxyObj_Interact.ProxyType.AmmoGrenade },
    };
    static readonly string[] RoomPickup = { "R_NN57", "M_SwordNexus", "R_NM46", "Shield_RiotLight", "R_Hrifle_NFAL", "R_NZucker", "R_Nc762", "M_SwordNexus", "R_NS-V.10", "R_Hrifle_NFAL" };

    static void Stations(Frame f, RoomObj room, int index)
    {
        var ip = AssetDatabase.LoadAssetAtPath<GameObject>(InteractPrefab);
        var mp = AssetDatabase.LoadAssetAtPath<GameObject>(ItemPrefab);
        var cover = room.GetComponentInChildren<ProxyObj_Cover>(true);
        var parent = cover != null ? cover.transform.parent : room.transform;
        if (ip == null) Debug.LogWarning("NexusProminence: " + InteractPrefab + " missing; no stations.");
        else
        {
            float u = 0.06f;
            foreach (var type in RoomStations[index])
            {
                bool placed = false;
                for (; u < 0.95f && !placed; u += 0.02f)
                {
                    float x = Mathf.Lerp(f.x0, f.x1, u);
                    var r = new Rect(x - 0.8f, f.y, 1.6f, 2.6f);
                    if (x - 0.8f < f.x0 + 0.8f || !WallFree(r)) continue;
                    var go = (GameObject)PrefabUtility.InstantiatePrefab(ip, room.gameObject.scene);
                    go.transform.SetParent(parent, true);
                    go.transform.position = new Vector3(x, f.y, f.zb - 0.5f);
                    var px = go.GetComponent<ProxyObj_Interact>();
                    px.Type = type;
                    go.name = "PX_Interact_" + type;
                    EditorUtility.SetDirty(px);
                    wallBusy.Add(r);
                    placed = true;
                }
                if (!placed)
                {
                    // wall full (room 3): stand it free on the floor, a step in front of the back wall,
                    // left of centre so it does not block the exit door
                    float x = Mathf.Lerp(f.x0, f.x1, 0.35f);
                    var go = (GameObject)PrefabUtility.InstantiatePrefab(ip, room.gameObject.scene);
                    go.transform.SetParent(parent, true);
                    go.transform.position = new Vector3(x, f.y, Mathf.Lerp(f.zf, f.zb, 0.8f));
                    var px = go.GetComponent<ProxyObj_Interact>();
                    px.Type = type;
                    go.name = "PX_Interact_" + type + "_Floor";
                    EditorUtility.SetDirty(px);
                    Debug.Log("NexusProminence: wall full; station " + type + " stands on the floor in " + room.name);
                }
            }
        }
        if (mp == null) { Debug.LogWarning("NexusProminence: " + ItemPrefab + " missing; no pickups."); return; }
        var it = (GameObject)PrefabUtility.InstantiatePrefab(mp, room.gameObject.scene);
        it.transform.SetParent(parent, true);
        it.transform.position = new Vector3(Mathf.Lerp(f.x0, f.x1, 0.2f), f.y + 0.1f, Mathf.Lerp(f.zf, f.zb, 0.3f));
        var mi = it.GetComponent<Marker_Item>();
        mi.MadCard = RoomPickup[index];
        mi.PhysicsOn = false;
        it.name = "Item_" + RoomPickup[index];
        EditorUtility.SetDirty(mi);
    }

    // Weapons from the film: street guards with rifles, hall agents with pistols and SMGs, riot guards with
    // a shield and a pistol, soldats with battle rifles, the odd agent with the Nexus sword. Card names are
    // the game's own (resources.assets, build 25615196).
    static void Loadout(Marker_Character m, CharacterTypes t, int room, int n)
    {
        string r = null, l = null;
        switch (t)
        {
            case CharacterTypes.Nexus1_Agent:
                if (room == 0) r = n % 3 == 2 ? "R_NN57" : "R_Nc762";
                else if (room == 1) r = n % 2 == 0 ? "R_NN57" : "R_NM46";
                else if (room >= 6 && n % 4 == 3) r = "M_SwordNexus";
                else r = new[] { "R_NN57", "R_NM46", "R_Nc762" }[(n + room) % 3];
                break;
            case CharacterTypes.Nexus2_Scout: r = room == 4 ? "R_Hrifle_NFAL" : "M_SwordNexus"; break;
            case CharacterTypes.Nexus4_Riot__Guard: r = "R_NN57"; l = "Shield_RiotLight"; break;
            case CharacterTypes.Nexus6_Soldat: r = "R_Hrifle_NFAL"; break;
            default: return; // support, engineer, scientist keep their own gear
        }
        m.ClearDefaultWeapons = true;
        m.Held_R = r; m.Held_L = l;
    }

    // Madness-map dressing on top of each room's set: the film's blood and bullet holes everywhere,
    // its own wall scrawl and chalk, plus one or two joke signs per room.
    // look proposal 1 (2026-11-11): the front third of every room was bare floor. Low debris there (rubble,
    // a fallen ceiling panel, casings, papers, a tipped bin) fills it without blocking: every piece stays under
    // 0.8 high, so Solidify gives it no collider, and nothing lands within 4 units of a door. Logged as NX_DEBRIS.
    const bool FrontDebris = true;
    static void Debris(Frame f, int index)
    {
        if (!FrontDebris) return;
        var r = new System.Random(500 + index);
        System.Func<float> R = () => (float)r.NextDouble();
        float zLo = f.zf + 0.6f, zHi = f.zf + Mathf.Max(1.5f, f.D * 0.33f);
        int clusters = 3 + index % 2, placed = 0;
        for (int c = 0, tries = 0; c < clusters && tries < 40; tries++)
        {
            float cx = Mathf.Lerp(f.x0 + 2f, f.x1 - 2f, (c + 0.2f + 0.6f * R()) / clusters), cz = Mathf.Lerp(zLo, zHi, R());
            if (f.Doors.Any(d => new Vector2(d.x - cx, d.z - cz).magnitude < 4f)) continue;
            c++;
            switch ((index + c) % 4)
            {
                case 0: // rubble heap
                    for (int k = 0; k < 5; k++)
                    {
                        float s = 0.25f + 0.35f * R();
                        var b = Box(f, "DebrisRubble", cx + (R() - 0.5f) * 1.6f, f.y + s * 0.35f, cz + (R() - 0.5f) * 1.0f, s, s * 0.7f, s * 0.9f, k % 2 == 0 ? "rubble" : "panel");
                        b.transform.rotation = Quaternion.Euler(R() * 20f, R() * 90f, R() * 20f);
                    }
                    break;
                case 1: // ceiling panel fallen flat, one corner propped up
                    var pn = Box(f, "DebrisPanel", cx, f.y + 0.2f, cz, 2.2f, 0.08f, 1.1f, "panel");
                    pn.transform.rotation = Quaternion.Euler(0, R() * 40f - 20f, 9f);
                    Box(f, "DebrisRubble", cx + 0.9f, f.y + 0.12f, cz, 0.3f, 0.24f, 0.3f, "rubble");
                    break;
                case 2: // spent casings and loose papers
                    for (int k = 0; k < 9; k++)
                    {
                        var cs = Box(f, "DebrisCasing", cx + (R() - 0.5f) * 1.8f, f.y + 0.03f, cz + (R() - 0.5f) * 1.2f, 0.06f, 0.06f, 0.16f, "steel");
                        cs.transform.rotation = Quaternion.Euler(0, R() * 180f, 0);
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        var pp = Box(f, "DebrisPaper", cx + (R() - 0.5f) * 2.2f, PaperY(f), cz + (R() - 0.5f) * 1.4f, 0.45f, 0.004f, 0.6f, "white");
                        pp.transform.rotation = Quaternion.Euler(0, R() * 180f, 0);
                    }
                    break;
                default: // tipped bin with spilled trash
                    var bin = Box(f, "DebrisBin", cx, f.y + 0.3f, cz, 0.6f, 0.6f, 0.75f, "dark");
                    bin.transform.rotation = Quaternion.Euler(0, R() * 60f - 30f, 0);
                    for (int k = 0; k < 4; k++) Box(f, "DebrisPaper", cx + 0.5f + R() * 1.2f, PaperY(f), cz + (R() - 0.5f) * 0.9f, 0.35f, 0.004f, 0.4f, k % 2 == 0 ? "white" : "stain");
                    break;
            }
            placed++;
        }
        Debug.Log("NX_DEBRIS room " + (index + 1) + ": " + placed + " clusters in the front third");
    }

    static void Gags(Frame f, int index)
    {
        var r = new System.Random(100 + index);
        // owner 2026-11-11: the film's street (room 1) is clean before the fight
        bool blood = index != 0;
        int splats = blood ? 3 + index % 3 : 0;
        for (int i = 0; i < splats; i++) Splat(f, 0.12f + (float)r.NextDouble() * 0.76f, 0.15f + (float)r.NextDouble() * 0.6f, 1.6f + (float)r.NextDouble() * 1.8f, i + index);
        WallDecal(f, "holes_" + (index % 2), 0.1f + 0.8f * (float)r.NextDouble(), 2.6f, 2.2f, 2.2f);
        WallDecal(f, "holes_" + ((index + 1) % 2), 0.1f + 0.8f * (float)r.NextDouble(), 3.4f, 1.8f, 1.8f);
        if (blood && index % 3 != 2) WallDecal(f, "splat_" + (index % 4), 0.2f + 0.6f * (float)r.NextDouble(), 2.2f, 2.4f, 2.4f, index * 31f);
        switch (index)
        {
            case 0:
                // the back wall is one window: the town sign goes on the hung sign, "no loitering" on the window head
                Decal(f, "sign_population", f.Cx, f.y + 6.0f, f.zb - 1.33f, 2.6f, 1.0f);
                Decal(f, "sign_noloiter", f.x1 - 3.5f, f.y + 7.1f, f.zb - 0.62f, 1.1f, 0.5f);
                WallDecal(f, "scrawl_0", 0.7f, 1.0f, 3.2f, 1.6f);
                break;
            case 1:
                WallDecal(f, "sign_incident", 0.95f, 3.0f, 1.5f, 1.25f);
                WallDecal(f, "sign_reward", 0.44f, 2.0f, 1.4f, 0.44f);
                WallDecal(f, "scrawl_1", 0.06f, 1.1f, 2.6f, 1.3f);
                break;
            case 2:
                WallDecal(f, "sign_number", 0.42f, 4.6f, 1.7f, 0.97f);
                WallDecal(f, "sign_receptionist", 0.72f, 4.7f, 2.4f, 1.12f);
                break;
            case 3:
                WallDecal(f, "sign_fragile", 0.5f, 4.4f, 2.0f, 0.94f);
                WallDecal(f, "sign_upside", 0.25f, 4.6f, 1.9f, 0.6f, 180f);
                Decal(f, "sign_wetfloor", f.x0 + 0.62f * f.W, f.y + 0.03f, f.zf + 0.35f * f.D, 1.0f, 1.25f, true, 15f);
                break;
            case 4:
                WallDecal(f, "sign_nopeek", 0.15f, 4.4f, 1.9f, 1.0f);
                WallDecal(f, "scrawl_0", 0.85f, 1.1f, 2.6f, 1.3f);
                break;
            case 5:
                WallDecal(f, "sign_doorgrunts", 0.08f, 3.0f, 1.6f, 1.08f);
                WallDecal(f, "sign_wetfloor", 0.92f, 1.0f, 0.8f, 1.0f);
                break;
            case 6:
                // the film's corridor chalkboard reads "Don't Blink"
                Box(f, "Chalkboard", f.x0 + 0.5f * f.W, f.y + 3.6f, f.zb - 0.4f, 4.2f, 1.6f, 0.06f, "dark");
                Decal(f, "sign_dontblink", f.x0 + 0.5f * f.W, f.y + 3.6f, f.zb - 0.45f, 3.9f, 1.25f); // on its board, never moved
                WallDecal(f, "scrawl_1", 0.85f, 1.2f, 2.6f, 1.3f);
                break;
            case 7:
                WallDecal(f, "sign_lostfound", 0.9f, 3.2f, 1.6f, 1.08f);
                break;
            case 8:
                WallDecal(f, "sign_lunch", 0.3f, 2.6f, 0.9f, 0.73f, 6f);
                WallDecal(f, "sign_coffee", 0.6f, 4.4f, 1.5f, 0.84f);
                break;
            case 9:
                WallDecal(f, "sign_authorized", 0.2f, 4.6f, 3.0f, 1.05f);
                WallDecal(f, "sign_selfdestruct", 0.62f, 2.0f, 1.3f, 0.81f);
                Box(f, "BigRedButton", f.x0 + 0.62f * f.W, f.y + 1.2f, f.zb - 0.6f, 0.5f, 0.3f, 0.3f, "red");
                break;
        }
    }

    static void Cyl(Frame f, string name, float x, float y, float z, float diameter, float height, string mat)
    {
        var s = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Object.DestroyImmediate(s.GetComponent<Collider>());
        s.name = "NX_" + name;
        s.transform.SetParent(f.Parent, true);
        s.transform.position = new Vector3(x, y, z);
        s.transform.localScale = new Vector3(diameter, height / 2, diameter);
        s.GetComponent<MeshRenderer>().sharedMaterial = f.M[mat];
        s.isStatic = true;
        Solidify(f, s, x, y - height / 2, z, diameter, height, diameter);
    }

    // Props that look solid must be solid (Workshop feedback: "cover that doesnt cover"). A piece standing
    // on the floor, at least 0.8 tall and 0.4 across both ways, gets a box collider on the Obstacle layer
    // (blocks bodies, not sight or aim). Never within 3.5 units of a door, so no exit is ever walled off.
    const bool SolidProps = true;
    const int ObstacleLayer = 13;
    static void Solidify(Frame f, GameObject s, float x, float bottom, float z, float sx, float sy, float sz)
    {
        if (!SolidProps || bottom - f.y > 0.15f || sy < 0.8f || sx < 0.4f || sz < 0.4f) return;
        if (sx > f.W * 0.6f || sz > f.D * 0.6f) return; // wall facings and floor sheets
        foreach (var d in f.Doors) if (new Vector2(d.x - x, d.z - z).magnitude < 3.5f) return;
        s.AddComponent<BoxCollider>();
        s.layer = ObstacleLayer;
        solidCount++;
    }
    static int solidCount;

    // Rightmost door of room k leads to the leftmost door of room k+1; the other doors stay spawn doors.
    // ---- encounters (fun pass 2): squads that drop in by rappel or crawl out of vents, reinforcement waves in
    // the control tower, timed spike strips. Spawn doors are PX_Door of a spawner type (VentSide, Rapel), linked to
    // nothing; Squad_Spawner (Marker_Squad) sends its wave through them. Set JuiceEncounters false to build without.
    const bool JuiceEncounters = true;
    const string DoorPrefab = "Assets/Library/Prefabs/Proxies/PX_Door.prefab";
    const string SquadPrefab = "Assets/Library/Prefabs/Markers/Squad_Spawner.prefab";
    const string TogglePrefab = "Assets/Library/Prefabs/Proxies/PX_Togglable.prefab";
    static readonly Dictionary<RoomObj, Frame> frames = new Dictionary<RoomObj, Frame>();

    static void Encounters(RoomObj room, Frame f, int index)
    {
        if (!JuiceEncounters) return;
        // the sample room's spare doors (not chained, spawning allowed), taken before any spawn door is added
        var spare = room.GetComponentsInChildren<ProxyObj_Door>(true)
            .Where(d => d.LinkedDoor == null && string.IsNullOrEmpty(d.LinkedScene) && !d.DisableSpawn && !d.gameObject.name.StartsWith("PX_Spawn_")).ToArray();
        DoorWaves(room, f, index, spare);
        switch (index)
        {
            case 3: // cargo bay: a rappel team drops in once the fight has started
                if (!FilmTotals) Squad(room, f, "CargoRappel", 14f, 3, Cap(3, 5), Units(new Foe(CharacterTypes.Nexus1_Agent, 3)),
                    WeaponLoadout_STP.LoadoutPools.NEXUS_T5, SpawnDoor(room, f, "Rappel_A", ProxyObj_Door.ProxyType.Rapel, 0.45f, 0.55f));
                else if (RappelBack) RappelTeam(room, f, index, "CargoRappel", 3, 0.30f, 0.40f);
                break;
            case 5: // shutter garage: two spike strips across the floor, out of step with each other
                Spikes(room, f, "A", 0.33f, 0.5f, 0f);
                Spikes(room, f, "B", 0.66f, FilmRoom6Ramp ? 0.4f : 0.5f, 120f); // zzj: off the pylon deck
                break;
            case 6: // spotlight corridor: agents crawl out of the wall vents; one spike strip mid-corridor
                if (!FilmTotals) Squad(room, f, "CorridorVents", 10f, 2, Cap(4, 5), Units(new Foe(CharacterTypes.Nexus1_Agent, 4)),
                    WeaponLoadout_STP.LoadoutPools.NEXUS_T5,
                    SpawnDoor(room, f, "Vent_A", ProxyObj_Door.ProxyType.VentSide, 0.4f, 0.97f),
                    SpawnDoor(room, f, "Vent_B", ProxyObj_Door.ProxyType.VentSide, 0.75f, 0.97f));
                Spikes(room, f, "A", 0.55f, 0.3f, 0f);
                break;
            case 9: // control tower: reinforcement waves by vent and rappel line, riot guards and soldats last
                if (!FilmTotals) Squad(room, f, "TowerReinforcements", 12f, 3, Cap(6, 7),
                    Units(new Foe(CharacterTypes.Nexus1_Agent, 5), new Foe(CharacterTypes.Nexus4_Riot__Guard, 2), new Foe(CharacterTypes.Nexus6_Soldat, 2)),
                    WeaponLoadout_STP.LoadoutPools.NEXUS_T7_Ranged,
                    SpawnDoor(room, f, "Vent_A", ProxyObj_Door.ProxyType.VentSide, 0.3f, 0.97f),
                    SpawnDoor(room, f, "Rappel_A", ProxyObj_Door.ProxyType.Rapel, 0.5f, 0.5f),
                    SpawnDoor(room, f, "Rappel_B", ProxyObj_Door.ProxyType.Rapel, 0.75f, 0.4f));
                else if (RappelBack) RappelTeam(room, f, index, "TowerRappel", 2, 0.5f, 0.5f);
                break;
        }
    }

    // Round 1 (build zk). The game lets a wave squad in only while the room's living enemies of that faction plus
    // the next squad stay at or under the wave's MaxUnitCount (Wave_Tandem.CanSpawnWithoutBreachingMax). With the
    // old caps the cargo bay rappel team (cap 3, squad 3, five placed enemies) could only come once the room was
    // empty, and the vents and tower waves late, so round 1 raised them (zj -> zk: 3 -> 5, 4 -> 5, 6 -> 7).
    // Round 8 retired the ReinforceWhileFighting toggle: its only squads (rappel, vents, tower) exist only with
    // FilmTotals false, and then the zk caps apply. The zj caps stay readable here and in git history.
    // Corrected in build zv: the cap is not tested for a spawner's first squad (see ArrivalsMidFight), so the
    // three-man rappel team was never held back by it; the cap only gates the later squads of a longer wave.
    static int Cap(int zj, int zk) { return zk; }

    // Build zzi, owner 2026-11-16: "the rappelling reinforcements never appear anymore". Cause: round 7 (build zq,
    // commit 83866a8) put every Encounters squad behind !FilmTotals, so since zq no Rapel door and no rappel squad
    // was built (build-zp.log has "squad CargoRappel", build-zq.log on have none); the README and the test lists
    // kept promising it. RappelBack brings back room 4's three-agent team and a two-agent line in room 10, timed
    // like the door squads (ArrivalsMidFight: after the alarm, a short delay, one squad, cap placed + door wave +
    // team) so a fast clear never leaves it armed. It adds 3 and 2 to those rooms' film totals, on the owner's word.
    // Set false to build zzh's encounters.
    const bool RappelBack = true;
    static void RappelTeam(RoomObj room, Frame f, int index, string name, int count, float u, float v)
    {
        int wave = FilmTotals && FilmTotalWaves[index] != null ? FilmTotalWaves[index].Sum(x => x.Count) : 0;
        var m = Squad(room, f, name, MidFightDelay[index] + 2f, count, PlacedCount(index) + wave + count, Units(new Foe(CharacterTypes.Nexus1_Agent, count)),
            index >= 6 ? WeaponLoadout_STP.LoadoutPools.NEXUS_T7_Ranged : WeaponLoadout_STP.LoadoutPools.NEXUS_T5,
            SpawnDoor(room, f, name + "_Line", ProxyObj_Door.ProxyType.Rapel, u, v));
        if (m != null) { m.SpawnRequirement = Marker_Squad.SpawnRules.MyFactionAlerted; EditorUtility.SetDirty(m); }
    }

    // Round 1 (build zk): every spare door of the sample room was a framed doorway nothing came through. DoorWaves
    // sends a small reinforcement squad through them in every room, sized to a saw-tooth curve (peaks at rooms 4,
    // 7 and 10, a breather in 5 and 8). The three quiet rooms send theirs only once the alarm is up
    // (SpawnRequirement MyFactionAlerted), so a quiet start stays quiet. Set DoorReinforcements false to build without.
    const bool DoorReinforcements = true;
    // Round 2 (build zl), owner 2026-11-14: "Don't betray the movie: the first room is only 2 enemies." Door waves
    // stay only in the rooms where the film shows enemies walking in during the fight (wanted hall, cargo bay,
    // spotlight corridor, control tower; film table in ai-docs/HANDOFF.md). Elsewhere the spare doors are shut
    // (DisableSpawn) so room 1 holds exactly its 2 agents. False restores round 1's door wave in every room.
    const bool FilmTrueArrivals = true;
    static readonly bool[] FilmDoorWaves = { false, true, false, true, false, false, true, false, false, true };
    // Build zv (film geometry chain, part A). A player reported the backup only on walking back through finished
    // rooms, and that the level can be finished without seeing any. Cause, read in the decompiled game
    // (Squad_DataAssigner.Update, Wave_Manager.CheckReadyToSpawn): the squad's timer is stamped on the first
    // frame its room is current and is only looked at while the room is current. Nothing resets it when the room
    // is left and nothing holds the player in the room, so with the zu delays (20, 18 and 20 s from entry, 8 s
    // from the alarm) a player who cleared the room sooner walked on with the squad still armed, and it came out
    // the moment the room was entered again. The unit cap was not the cause: the first squad of a spawner always
    // enters (the cap is only tested while units of that wave are alive), and every zu door wave is one squad.
    // ArrivalsMidFight makes every door squad wait for the alarm (SpawnRequirement MyFactionAlerted: placed
    // enemies alive and not Unaware, so an emptied room never arms it), then MidFightDelay seconds; keeps the
    // whole wave in one squad so nothing is left to come later; and sets the cap to placed + squad. Counts are
    // unchanged. false = the zu timing; sobj-zu in the lab is the zu build.
    const bool ArrivalsMidFight = true;
    static readonly float[] MidFightDelay = { 2f, 2f, 2f, 4f, 3f, 3f, 3f, 3f, 3f, 3f };
    static int PlacedCount(int index)
    {
        var foes = StagedFights && index < FilmCountFoes.Length && FilmCountFoes[index] != null ? FilmCountFoes[index] : Rooms[index].Foes;
        if (FilmTotals && FilmTotalFoes[index] != null) foes = FilmTotalFoes[index];
        return foes.Sum(x => x.Count);
    }
    struct DoorWave { public float Delay; public bool OnAlarm; public int Cap; public Foe[] Foes; }
    static readonly DoorWave[] DoorWavePlan =
    {
        new DoorWave { Delay = 4f, OnAlarm = true, Cap = 3, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new DoorWave { Delay = 4f, OnAlarm = true, Cap = 4, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus2_Scout, 1) } },
        new DoorWave { Delay = 5f, OnAlarm = true, Cap = 4, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus3_Support, 1) } },
        new DoorWave { Delay = 20f, Cap = 5, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus4_Riot__Guard, 1) } },
        new DoorWave { Delay = 10f, Cap = 4, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new DoorWave { Delay = 10f, Cap = 4, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus5_Engineer, 1) } },
        new DoorWave { Delay = 18f, Cap = 5, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus4_Riot__Guard, 1) } },
        new DoorWave { Delay = 10f, Cap = 3, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2) } },
        new DoorWave { Delay = 8f, Cap = 5, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 3), new Foe(CharacterTypes.Nexus3_Support, 1) } },
        new DoorWave { Delay = 20f, Cap = 6, Foes = new[] { new Foe(CharacterTypes.Nexus1_Agent, 2), new Foe(CharacterTypes.Nexus6_Soldat, 2) } },
    };

    static void DoorWaves(RoomObj room, Frame f, int index, ProxyObj_Door[] spare)
    {
        if (!DoorReinforcements || index >= DoorWavePlan.Length) return;
        if (FilmTrueArrivals && !FilmDoorWaves[index])
        {
            // the film shows no one walking in here: keep the spare doors shut instead of inventing a squad
            foreach (var d in spare) { d.DisableSpawn = true; EditorUtility.SetDirty(d); }
            Debug.Log("NexusProminence: " + room.name + " no door wave (film shows none); " + spare.Length + " spare doors shut");
            return;
        }
        if (spare.Length == 0) { Debug.LogWarning("NexusProminence: " + room.name + " has no spare door for its door wave"); return; }
        var w = DoorWavePlan[index];
        if (FilmTotals && FilmTotalWaves[index] != null) { w.Foes = FilmTotalWaves[index]; w.Cap = Mathf.Min(w.Cap, 4); if (index == 1) w.Delay = 8f; }
        foreach (var d in spare) { d.SerialNumber = "NXP_" + room.name + "_door_" + System.Array.IndexOf(spare, d); EditorUtility.SetDirty(d); }
        int squadSize = 2, units = w.Foes.Sum(x => x.Count), placed = PlacedCount(index);
        if (ArrivalsMidFight) { w.Delay = MidFightDelay[index]; w.OnAlarm = true; squadSize = Mathf.Max(2, units); w.Cap = placed + units; }
        var m = Squad(room, f, "DoorWave", w.Delay, squadSize, w.Cap, Units(w.Foes),
            index >= 6 ? WeaponLoadout_STP.LoadoutPools.NEXUS_T7_Ranged : WeaponLoadout_STP.LoadoutPools.NEXUS_T5, spare);
        if (m != null && w.OnAlarm) { m.SpawnRequirement = Marker_Squad.SpawnRules.MyFactionAlerted; EditorUtility.SetDirty(m); }
        Debug.Log("NexusProminence: " + room.name + " arrival: placed " + placed + ", squad " + units + " in squads of " + squadSize + ", cap " + w.Cap
            + ", " + w.Delay + " s " + (w.OnAlarm ? "after the alarm" : "after entry") + (ArrivalsMidFight ? " (ArrivalsMidFight)" : ""));
        // Round 6 sent the hall's placed scout in by Marker_Character.SpawnFromDoor; round 8 retired that. Decompiled
        // game: Converter_Marker copies SpawnFromDoor to Char_DataAssigner.SpawnToEntranceOrVehicle, and the character
        // spawns as soon as its room becomes current (CanSpawnOnSceneStart, Update) and is handed straight to the door
        // (GiveCharacterToMe): it walks in on room entry, with no delay and no alarm condition. The film's walk-in is
        // mid-fight, so the FilmTotals door squad (MyFactionAlerted, then SecondsInRoomTilSpawn) is the right tool.
        // The zp walker: sobj-zp.
    }

    static List<WaveCharacter_STP> Units(params Foe[] foes)
    {
        // GenerationScale 0.5 brings the later lines (riot guards, soldats) in toward the end of the wave
        return foes.Select(x => new WaveCharacter_STP { Character = x.Type, Total = x.Count, GenerationScale = 0.5f, MadCard = "NONE" }).ToList();
    }

    static ProxyObj_Door SpawnDoor(RoomObj room, Frame f, string name, ProxyObj_Door.ProxyType type, float u, float v)
    {
        var src = AssetDatabase.LoadAssetAtPath<GameObject>(DoorPrefab);
        if (src == null) { Debug.LogWarning("NexusProminence: " + DoorPrefab + " missing; no " + name); return null; }
        var any = room.GetComponentsInChildren<ProxyObj_Door>(true).FirstOrDefault();
        var go = (GameObject)PrefabUtility.InstantiatePrefab(src, room.gameObject.scene);
        go.transform.SetParent(any != null ? any.transform.parent : room.transform, true);
        go.transform.position = new Vector3(Mathf.Lerp(f.x0 + 2f, f.x1 - 2f, u), f.y, Mathf.Lerp(f.zf + 2f, f.zb - 0.6f, v));
        go.transform.rotation = Quaternion.identity;
        go.name = "PX_Spawn_" + name;
        var d = go.GetComponent<ProxyObj_Door>();
        d.Type = type; d.LinkedDoor = null; d.LinkedScene = ""; d.DisableSpawn = false;
        d.SerialNumber = "NXP_" + room.name + "_" + name;
        EditorUtility.SetDirty(d);
        // set dressing: a rope from the ceiling for a rappel point, a grille low on the back wall for a vent
        var p = go.transform.position;
        if (type == ProxyObj_Door.ProxyType.Rapel)
        {
            Box(f, "RappelRope", p.x, f.y + 4.4f, p.z, 0.06f, 4.0f, 0.06f, "dark");
            // owner 2026-11-11: the team slid down into a pallet stack. Keep the landing clear: push any cover
            // within 3.5 units of the rope sideways, toward whichever wall is farther, until it is clear.
            foreach (var c in room.GetComponentsInChildren<ProxyObj_Cover>(true))
            {
                var cp = c.transform.position;
                float dir = cp.x >= p.x ? 1f : -1f;
                if ((dir > 0 ? f.x1 - p.x : p.x - f.x0) < 4f) dir = -dir;
                bool moved = false;
                for (int guard = 0; guard < 20 && new Vector2(cp.x - p.x, cp.z - p.z).magnitude < 3.5f; guard++) { cp.x += dir * 0.5f; moved = true; }
                if (!moved) continue;
                cp.x = Mathf.Clamp(cp.x, f.x0 + 1.5f, f.x1 - 1.5f);
                c.transform.position = cp;
                EditorUtility.SetDirty(c);
                Debug.Log("NexusProminence: NX_LANDING moved " + c.gameObject.name + " off " + name + " in " + room.name);
            }
        }
        else
        {
            Box(f, "VentFrame", p.x, f.y + 0.75f, f.zb - 0.2f, 1.7f, 1.3f, 0.06f, "steel"); // in front of the band and skirting
            for (int k = 0; k < 4; k++) Box(f, "VentSlat", p.x, f.y + 0.35f + k * 0.27f, f.zb - 0.24f, 1.4f, 0.1f, 0.04f, "dark");
        }
        return d;
    }

    static Marker_Squad Squad(RoomObj room, Frame f, string name, float delay, int squadSize, int maxOnScreen,
        List<WaveCharacter_STP> units, WeaponLoadout_STP.LoadoutPools guns, params ProxyObj_Door[] doors)
    {
        var src = AssetDatabase.LoadAssetAtPath<GameObject>(SquadPrefab);
        if (src == null) { Debug.LogWarning("NexusProminence: " + SquadPrefab + " missing; no " + name); return null; }
        var go = (GameObject)PrefabUtility.InstantiatePrefab(src, room.gameObject.scene);
        go.transform.SetParent(room.CharacterFolder != null ? room.CharacterFolder.transform : room.transform, false);
        go.transform.position = new Vector3(f.Cx, f.y, f.Cz); // position does not matter for a squad spawner
        go.name = "Squad_" + name;
        var m = go.GetComponent<Marker_Squad>();
        m.SpawnEnabled = true; m.Faction = Factions.Nexus; m.Alertness = AlertStatus.Combat;
        m.SerialNumber = "NXP_" + name;
        m.SecondsInRoomTilSpawn = delay;
        m.SpawnFromDoors = doors.Where(d => d != null).Cast<ProxyObj_REPOSITORY>().ToList();
        m.SpawnRequirement = Marker_Squad.SpawnRules.None;
        m.NeverendingWaves = false;
        m.WaveSpawnType = Marker_Squad.TandemTypes.NewTandem;
        m.Wave = new WaveSpawn_STP { MaxSquadSize = squadSize, MaxUnitCount = maxOnScreen, WaveUnits = units,
            WeaponStock = new List<WeaponLoadout_STP> { new WeaponLoadout_STP { MadCard = "NONE", Loadout = guns, Count = units.Sum(x => x.Total) } } };
        EditorUtility.SetDirty(m);
        Debug.Log("NexusProminence: " + room.name + " squad " + name + " (" + units.Sum(x => x.Total) + " units, " + m.SpawnFromDoors.Count + " doors, after " + delay + " s, cap " + maxOnScreen + ")");
        return m;
    }

    // Build zzi, owner 2026-11-16: "the spikes never worked". Cause (decompiled game): the converter swaps the
    // stock PX_Togglable for the library strip, a Toggleable_Main whose animator and hazard act only when its own
    // SetState changes (Init sets it Up once at load). The CO_ timer is a second Toggleable_Main; its SetState only
    // calls SetActive on its Up objects, so the strip was shown and hidden and never rose through anyone, and
    // nothing else ever calls the strip's SetState. SpikesHurt adds an invisible trigger with Hazard_Core (the
    // game's Hazard_Main: damage on OnTriggerEnter while enabled; OnceTilReset clears its hit list on OnEnable) to
    // the same Up list, so each time the strip shows, whoever stands in it or walks in is hit once. Set false to
    // build zzh's strips (look only).
    // Build zzk, owner 2026-11-16: zzi's spikes still never hurt. Cause (decompiled game): with HitAvoid = Normal
    // Hazard_Core.cs:22 sets hitRange = (int)HitAvoid = 1, Hazard_Main.cs:286 marks the hit DamageSource.Terrain
    // (not IgnoreThreshold/KillHit), and Combat_CharacterHit.cs:540 skips the dodge check only when range == -1, so
    // an alert character's tac bar turned every hit into a TacDodge (no damage, no log line). Fix as the game's own
    // Samples/CO_Vessel.prefab: HitAvoid Undodgable (-1), damageEffect IgnoreThreshold (2), trigger on layer 12.
    const bool SpikesHurt = true;
    static GameObject SpikeBlades(Frame f, RoomObj room, GameObject strip, string name)
    {
        var b = new GameObject("NX_SpikeBlades_" + name);
        b.layer = 12;
        b.transform.SetParent(strip.transform.parent, true);
        b.transform.position = strip.transform.position + new Vector3(0f, 0.6f, 0f);
        var bc = b.AddComponent<BoxCollider>(); bc.isTrigger = true; bc.size = new Vector3(3.0f, 1.2f, 1.2f);
        var rb = b.AddComponent<Rigidbody>(); rb.isKinematic = true; rb.useGravity = false;
        var h = b.AddComponent<Hazard_Core>();
        h.contactDmg = 35f; h.damageEffect = DamageEffect.IgnoreThreshold; h.armorPierce = 1f; h.bounceAway = 6f;
        h.dmgType = DamageType.Piercing; h.myHitFrequency = HitFrequency.OnceTilReset; h.HitAvoid = AttackQuality.Undodgable;
        h.myEffects = new List<Effects> { Effects.IgnoreDeadguys };
        EditorUtility.SetDirty(h);
        Debug.Log("NX_SPIKES " + room.name + " " + name + " hazard trigger at " + b.transform.position);
        return b;
    }

    // Build zzl, owner on zzk 2026-11-16: the spikes hurt but never visibly shoot up. Cause (decompiled game):
    // the timer shows and hides the library strip with SetActive (Toggleable_Main.SetUpDownActiveObjects), and
    // Unity resets an Animator to its default state and parameters when its object is re-enabled, so the strip
    // came back in its holed Down pose every time; its own SetState, the only thing that plays Up, is never called.
    // SpikesRise drops the library strip for our own rig, driven the way the SceneTools sample
    // Samples/CO_Togglable.prefab is: the timer's CustomObj_Togglable gets LinkedAnimator = the rig's Animator,
    // whose controller overrides Library/AnimControllers/Togglables/Toggleable_Base.controller with our Up clip
    // (blades scale up in 0.08 s) and Down clip (back down in 0.4 s). Toggleable_Main.SetState sets ToggleMe on
    // it; the rig is never hidden. Set false to build zzk's strips.
    const bool SpikesRise = true;
    const string ToggleBaseController = "Assets/Library/AnimControllers/Togglables/Toggleable_Base.controller";
    static Mesh spikeMesh;
    static Mesh SpikeMesh()
    {
        if (spikeMesh != null) return spikeMesh;
        string path = MatDir + "/NX_SpikeMesh.asset";
        if (AssetDatabase.LoadAssetAtPath<Mesh>(path) != null) AssetDatabase.DeleteAsset(path);
        // a four-sided blade: square base 0.12, tip 0.7 up (flat-shaded: each face its own vertices)
        float b = 0.06f, h = 0.7f; var tip = new Vector3(0f, h, 0f);
        var c = new[] { new Vector3(-b, 0f, -b), new Vector3(b, 0f, -b), new Vector3(b, 0f, b), new Vector3(-b, 0f, b) };
        var v = new List<Vector3>(); var tri = new List<int>();
        for (int i = 0; i < 4; i++) { int k = v.Count; v.Add(c[i]); v.Add(tip); v.Add(c[(i + 1) % 4]); tri.AddRange(new[] { k, k + 1, k + 2 }); }
        spikeMesh = new Mesh { name = "NX_SpikeMesh" };
        spikeMesh.SetVertices(v); spikeMesh.SetTriangles(tri, 0);
        spikeMesh.uv = v.Select(q => new Vector2(q.x + 0.5f, q.y)).ToArray();
        spikeMesh.RecalculateNormals(); spikeMesh.RecalculateBounds();
        AssetDatabase.CreateAsset(spikeMesh, path);
        return spikeMesh;
    }

    static AnimatorOverrideController spikeAnim;
    static AnimatorOverrideController SpikeAnim()
    {
        if (spikeAnim != null) return spikeAnim;
        var baseCtl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(ToggleBaseController);
        if (baseCtl == null) { Debug.LogWarning("NexusProminence: " + ToggleBaseController + " missing; spikes cannot rise"); return null; }
        AnimationClip Clip(string n, float t, float from, float to)
        {
            string p = MatDir + "/" + n + ".anim";
            if (AssetDatabase.LoadAssetAtPath<AnimationClip>(p) != null) AssetDatabase.DeleteAsset(p);
            var clip = new AnimationClip { name = n, frameRate = 60f };
            clip.SetCurve("Blades", typeof(Transform), "m_LocalScale.y", AnimationCurve.EaseInOut(0f, from, t, to));
            clip.SetCurve("Blades", typeof(Transform), "m_LocalScale.x", AnimationCurve.Constant(0f, t, 1f));
            clip.SetCurve("Blades", typeof(Transform), "m_LocalScale.z", AnimationCurve.Constant(0f, t, 1f));
            AssetDatabase.CreateAsset(clip, p);
            return clip;
        }
        var up = Clip("NX_SpikesUp", 0.08f, 0.02f, 1f);
        var down = Clip("NX_SpikesDown", 0.4f, 1f, 0.02f);
        string op = MatDir + "/NX_Spikes.overrideController";
        if (AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(op) != null) AssetDatabase.DeleteAsset(op);
        var o = new AnimatorOverrideController(baseCtl) { name = "NX_Spikes" };
        var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        o.GetOverrides(pairs);
        for (int i = 0; i < pairs.Count; i++)
            pairs[i] = new KeyValuePair<AnimationClip, AnimationClip>(pairs[i].Key, pairs[i].Key.name.EndsWith("Up") ? up : down);
        o.ApplyOverrides(pairs);
        AssetDatabase.CreateAsset(o, op);
        Debug.Log("NX_SPIKEANIM overrides " + string.Join(", ", pairs.Select(q => q.Key.name + "->" + q.Value.name)));
        return spikeAnim = o;
    }

    // Our strip: dark plate with red slots, a "Blades" child of 2 x 10 blades (scaled to 0.02 = down), and the
    // Animator the timer drives. Visual only: no colliders (Box under 0.8 tall is not solidified).
    static Animator SpikeRig(Frame f, Transform parent, Vector3 at, string name)
    {
        var rig = new GameObject("NX_SpikeRig_" + name);
        rig.transform.SetParent(parent, true); rig.transform.position = at;
        var plate = Box(f, "SpikePlate", at.x, f.y + 0.012f, at.z, 3.0f, 0.02f, 1.1f, "dark");
        plate.transform.SetParent(rig.transform, true);
        var blades = new GameObject("Blades");
        blades.transform.SetParent(rig.transform, false);
        blades.transform.localPosition = new Vector3(0f, f.y + 0.02f - at.y, 0f);
        blades.transform.localScale = new Vector3(1f, 0.02f, 1f);
        var mesh = SpikeMesh();
        for (int row = 0; row < 2; row++)
            for (int k = 0; k < 10; k++)
            {
                float x = -1.35f + k * 0.3f, z = row == 0 ? -0.22f : 0.22f;
                var slot = Box(f, "SpikeSlot", at.x + x, f.y + 0.023f, at.z + z, 0.16f, 0.004f, 0.16f, "red");
                slot.transform.SetParent(rig.transform, true);
                var s = new GameObject("NX_Spike");
                s.transform.SetParent(blades.transform, false);
                s.transform.localPosition = new Vector3(x, 0f, z);
                s.AddComponent<MeshFilter>().sharedMesh = mesh;
                var r = s.AddComponent<MeshRenderer>(); r.sharedMaterial = f.M["steel"];
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        var an = rig.AddComponent<Animator>();
        an.runtimeAnimatorController = SpikeAnim(); an.applyRootMotion = false;
        return an;
    }

    // Spike strip that rises and drops on a timer (automator times are frames, 60 per second).
    static void Spikes(RoomObj room, Frame f, string name, float u, float v, float offsetFrames)
    {
        if (SpikesRise) { SpikesRig(room, f, name, u, v, offsetFrames); return; }
        var src = AssetDatabase.LoadAssetAtPath<GameObject>(TogglePrefab);
        if (src == null) { Debug.LogWarning("NexusProminence: " + TogglePrefab + " missing; no spikes"); return; }
        var any = room.GetComponentsInChildren<ProxyObj_Door>(true).FirstOrDefault();
        var go = (GameObject)PrefabUtility.InstantiatePrefab(src, room.gameObject.scene);
        go.transform.SetParent(any != null ? any.transform.parent : room.transform, true);
        go.transform.position = new Vector3(Mathf.Lerp(f.x0 + 2f, f.x1 - 2f, u), f.y, Mathf.Lerp(f.zf + 2f, f.zb - 1.5f, v));
        go.name = "PX_Spikes_" + name;
        var t = go.GetComponent<ProxyObj_Togglable>();
        t.Type = ProxyObj_Togglable.ProxyType.Spikes; t.StartUp = true; t.PlayFirstAnim = false; t.ResetPathNodes = false;
        t.SerialNumber = "NXP_" + room.name + "_Spikes_" + name;
        EditorUtility.SetDirty(t);
        // The game swaps a stock PX_Togglable for a new library object and leaves add-ons behind on the proxy,
        // where Toggleable_Automator.Init finds no Toggleable_Main and throws (seen in play 2026-11-11). So the
        // timer drives an invisible CustomObj_Togglable (the game adds Toggleable_Main to that same object)
        // whose Up object is the stock strip: the converter swaps the entry for the created spikes.
        var drv = new GameObject("CO_NX_SpikeTimer_" + name);
        drv.transform.SetParent(go.transform.parent, true);
        drv.transform.position = go.transform.position;
        var c = drv.AddComponent<CustomObj_Togglable>();
        c.StartUp = true; c.PlayFirstAnim = false; c.ResetPathNodes = false;
        c.SerialNumber = "NXP_" + room.name + "_SpikeTimer_" + name;
        c.UpObjs = new List<GameObject> { go }; c.DownObjs = new List<GameObject>();
        if (SpikesHurt) c.UpObjs.Add(SpikeBlades(f, room, go, name));
        c.UpSound = ""; c.DownSound = "";
        EditorUtility.SetDirty(c);
        var a = drv.AddComponent<CustomAddon_ToggleAutomator>();
        a.AutomatorEnabled = true; a.RunInActiveRoomOnly = true; a.OnlyRunDuringWaves = false;
        a.TimeStayDown = 150; a.TimeStayUp = 90; a.StartTime = 60 + offsetFrames;
        EditorUtility.SetDirty(a);
        // warning paint: a hazard-striped plate under the strip
        var q = go.transform.position;
        // zzh (owner's glitch shot 1): the paint lay under the game's strip and its holed plate flickered through it.
        // Now two striped bands frame the strip front and back and nothing is drawn under it.
        foreach (var side in new[] { -1f, 1f })
            for (int k = 0; k < 6; k++) Box(f, "SpikeHazard", q.x - 1.5f + k * 0.6f, f.y + 0.01f, q.z + side * 0.72f, 0.3f, 0.004f, 0.28f, k % 2 == 0 ? "red" : "dark");
    }

    // SpikesRise: same place, timer and hazard as Spikes, but the timer drives our rig's Animator directly.
    static void SpikesRig(RoomObj room, Frame f, string name, float u, float v, float offsetFrames)
    {
        var any = room.GetComponentsInChildren<ProxyObj_Door>(true).FirstOrDefault();
        var parent = any != null ? any.transform.parent : room.transform;
        var at = new Vector3(Mathf.Lerp(f.x0 + 2f, f.x1 - 2f, u), f.y, Mathf.Lerp(f.zf + 2f, f.zb - 1.5f, v));
        var an = SpikeRig(f, parent, at, name);
        var drv = new GameObject("CO_NX_SpikeTimer_" + name);
        drv.transform.SetParent(parent, true);
        drv.transform.position = at;
        var c = drv.AddComponent<CustomObj_Togglable>();
        c.StartUp = false; c.PlayFirstAnim = false; c.ResetPathNodes = false;
        c.SerialNumber = "NXP_" + room.name + "_SpikeTimer_" + name;
        c.LinkedAnimator = an;
        c.UpObjs = new List<GameObject>(); c.DownObjs = new List<GameObject>();
        // SpikeBlades only reads the strip's parent and position
        if (SpikesHurt) c.UpObjs.Add(SpikeBlades(f, room, an.gameObject, name));
        c.UpSound = ""; c.DownSound = "";
        EditorUtility.SetDirty(c);
        var a = drv.AddComponent<CustomAddon_ToggleAutomator>();
        a.AutomatorEnabled = true; a.RunInActiveRoomOnly = true; a.OnlyRunDuringWaves = false;
        a.TimeStayDown = 150; a.TimeStayUp = 90; a.StartTime = 60 + offsetFrames;
        EditorUtility.SetDirty(a);
        foreach (var side in new[] { -1f, 1f })
            for (int k = 0; k < 6; k++) Box(f, "SpikeHazard", at.x - 1.5f + k * 0.6f, f.y + 0.01f, at.z + side * 0.72f, 0.3f, 0.004f, 0.28f, k % 2 == 0 ? "red" : "dark");
        Debug.Log("NX_SPIKERIG " + room.name + " " + name + " at " + at + " animator " + (an.runtimeAnimatorController != null ? an.runtimeAnimatorController.name : "NONE"));
    }

    static void ChainDoors(List<RoomObj> rooms)
    {
        for (int k = 0; k + 1 < rooms.Count; k++)
        {
            var a = rooms[k].GetComponentsInChildren<ProxyObj_Door>(true).OrderBy(d => d.transform.position.x).LastOrDefault();
            var b = rooms[k + 1].GetComponentsInChildren<ProxyObj_Door>(true).OrderBy(d => d.transform.position.x).FirstOrDefault();
            if (a == null || b == null) { Debug.LogWarning("NexusProminence: no door to link between " + rooms[k].name + " and " + rooms[k + 1].name); continue; }
            a.LinkedDoor = b; b.LinkedDoor = a;
            a.DisableSpawn = b.DisableSpawn = true;
            a.SerialNumber = "NXP_door_" + k + "_out"; b.SerialNumber = "NXP_door_" + (k + 1) + "_in";
            a.gameObject.name = "PX_Door_to_" + rooms[k + 1].name; b.gameObject.name = "PX_Door_from_" + rooms[k].name;
            EditorUtility.SetDirty(a); EditorUtility.SetDirty(b);
        }
    }
}
