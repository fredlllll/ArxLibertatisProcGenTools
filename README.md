[![NuGet Version](https://img.shields.io/nuget/v/ArxLibertatisProcGenTools)](https://www.nuget.org/packages/ArxLibertatisProcGenTools)


this is a library that is intended to either be referenced from nuget, or imported in powershell scripts (needs powershell 7+)

you can modify or generate levels for arx fatalis with it, programatically

there is an example script called example.ps1

there is also an examplecsg.ps1 which also includes the use of the https://github.com/talanc/DotNetCsg library to use CSG to create level geometry

run docs.ps1 to display the following documentation in plain text
 

# Powershell Documentation:
## This is a listing of all relevant classes and functions for generating levels in powershell scripts
### Mesh Generators are classes that will generate polygons that will be added to the level. These are available:
**ArxLibertatisProcGenTools.Generators.Plane.FloorGenerator**<br>
Generates a flat surface of a certain size around a center, using a texture generator<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;FloorGenerator()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;ITextureGenerator TextureGenerator<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector2 Size<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Center<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IEnumerable\`1 GetPolygons()<br>
<br>
**ArxLibertatisProcGenTools.Generators.Plane.QuadGenerator**<br>
Generates a single quad in a certain orientation<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;QuadGenerator()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Center<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Width<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Height<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Normal<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 WorldUp<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single MinU<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single MaxU<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single MinV<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single MaxV<br>
&nbsp;&nbsp;&nbsp;&nbsp;Int16 Room<br>
&nbsp;&nbsp;&nbsp;&nbsp;PolyType PolyType<br>
&nbsp;&nbsp;&nbsp;&nbsp;String TexturePath<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single TransVal<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IEnumerable\`1 GetPolygons()<br>
&nbsp;&nbsp;&nbsp;&nbsp;Polygon GetPolygon()<br>
<br>
**ArxLibertatisProcGenTools.Generators.Mesh.CSGGenerator**<br>
Adds CSG solids to the level<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;CSGGenerator()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Solid CsgSolid<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 PositionOffset<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Scale<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 UVScale<br>
&nbsp;&nbsp;&nbsp;&nbsp;PolyType PolyType<br>
&nbsp;&nbsp;&nbsp;&nbsp;Int16 Room<br>
&nbsp;&nbsp;&nbsp;&nbsp;ITextureGenerator TextureGenerator<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IEnumerable\`1 GetPolygons()<br>
&nbsp;&nbsp;&nbsp;&nbsp;BoundingBox GetSolidBounds()<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetSolidSize()<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetFinalSize()<br>
<br>
**ArxLibertatisProcGenTools.Generators.Mesh.OBJImporter**<br>
Imports obj files, only supports triangulated files. set Pos/Rot/Scale using WorldMatrix<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;OBJImporter(System.String objFilePath, System.String mtlFilePath)<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Matrix4x4 WorldMatrix<br>
&nbsp;&nbsp;&nbsp;&nbsp;Int16 Room<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IEnumerable\`1 GetPolygons()<br>
<br>
### Light generators generate lights that will be added to the level. These are available:
**ArxLibertatisProcGenTools.Generators.Light.RandomLightGenerator**<br>
Generates lights at random positions within its shape, with random attributes dictated by their shapes<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;RandomLightGenerator()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Int32 Count<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape PositionShape<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape ColorShape<br>
&nbsp;&nbsp;&nbsp;&nbsp;IValue FalloffStart<br>
&nbsp;&nbsp;&nbsp;&nbsp;IValue FalloffEnd<br>
&nbsp;&nbsp;&nbsp;&nbsp;IValue Intensity<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IEnumerable\`1 GetLights()<br>
<br>
### Texture generators generate texture names depending on position. These are available:
**ArxLibertatisProcGenTools.Generators.Texture.SingleTexture**<br>
Returns just a fixed texture<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;SingleTexture(System.String path)<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;String GetTexturePath(Int32 polygonIndex)<br>
<br>
### Modifiers modify the currently existing polygons. These are available:
**ArxLibertatisProcGenTools.Modifiers.DetailEnhancer**<br>
Generates extra polygons in an area so it can be sculpted to a greater detail by modifiers<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;DetailEnhancer()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape Shape<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Void Apply(ArxLibertatisEditorIO.WellDoneIO.WellDoneArxLevel wdl)<br>
<br>
**ArxLibertatisProcGenTools.Modifiers.Rumble**<br>
Adds random offsets(noise) to polygons in an area<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Rumble()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Magnitude<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape Shape<br>
&nbsp;&nbsp;&nbsp;&nbsp;IValue NoiseValue<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Void Apply(ArxLibertatisEditorIO.WellDoneIO.WellDoneArxLevel wdl)<br>
<br>
### Shapes can be used in a lot of ways to shape the output of other classes. These are available:
**ArxLibertatisProcGenTools.Shapes.Cuboid**<br>
The shape of a cube<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Cuboid()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Min<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Max<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
**ArxLibertatisProcGenTools.Shapes.Everywhere**<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Everywhere()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
**ArxLibertatisProcGenTools.Shapes.FixedVector**<br>
Returns a fixed value<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;FixedVector()<br>
&nbsp;&nbsp;&nbsp;&nbsp;FixedVector(System.Numerics.Vector3 value)<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Value<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
**ArxLibertatisProcGenTools.Shapes.NullShape**<br>
Always returns zero<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;NullShape()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
**ArxLibertatisProcGenTools.Shapes.MultiplyShape**<br>
returns the multiplication of two shapes<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;MultiplyShape()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape Shape1<br>
&nbsp;&nbsp;&nbsp;&nbsp;IShape Shape2<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
**ArxLibertatisProcGenTools.Shapes.Sphere**<br>
the shape of a sphere<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Sphere()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 Center<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Radius<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Falloff<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;the falloff area around the edge of the sphere (r:500, f:200 => falloff from 400 to 600)<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetAffectedness(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;Vector3 GetRandomPosition()<br>
<br>
### Values are similar to Shapes, just one dimensional. These are available:
**ArxLibertatisProcGenTools.Values.FixedValue**<br>
A fixed value<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;FixedValue()<br>
&nbsp;&nbsp;&nbsp;&nbsp;FixedValue(Single value)<br>
&nbsp;&nbsp;&nbsp;&nbsp;FixedValue(Double value)<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Value<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single GetValue(System.Numerics.Vector3 input)<br>
<br>
**ArxLibertatisProcGenTools.Values.NullValue**<br>
Always returns zero<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;NullValue()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single GetValue(System.Numerics.Vector3 input)<br>
<br>
**ArxLibertatisProcGenTools.Values.RandomValue**<br>
random value (uniform)<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;RandomValue()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Min<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single Max<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single GetValue(System.Numerics.Vector3 input)<br>
<br>
**ArxLibertatisProcGenTools.Values.SimplexNoiseValue**<br>
random value (simplex, like perlin but better), get Noise property to change parameters<br>
&nbsp;&nbsp;Constructors:<br>
&nbsp;&nbsp;&nbsp;&nbsp;SimplexNoiseValue()<br>
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Simplex Noise<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;Single GetValue(System.Numerics.Vector3 input)<br>
<br>
### In addition to these classes, there is the ScriptFunc class, which contains static functions that make scripting easier:
&nbsp;&nbsp;Properties:<br>
&nbsp;&nbsp;&nbsp;&nbsp;static WellDoneArxLevel Level<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Boolean SkipLighting<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Skip the lighting calculation on save<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Boolean Markdown<br>
&nbsp;&nbsp;Methods:<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void Clear()<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;deletes the level currently in memory and resets parameters. required cause its persistent during the powershell session<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void SetDataDir(System.String dataDir)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Set data dir that the level is saved to (and can be loaded from)<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void LoadLevel(System.String name)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loads a level to be modified<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void SaveLevel(System.String name)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Saves the level<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void Apply(ArxLibertatisProcGenTools.Generators.IMeshGenerator meshGenerator)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Applies the Mesh Generator to the level<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void Apply(ArxLibertatisProcGenTools.Modifiers.IModifier meshModifier)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Applies the Modifier to the level<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void Apply(ArxLibertatisProcGenTools.Generators.ILightGenerator lightGenerator)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Applies the Light Generator to the level<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void SetLightingProfile(ArxLibertatisLightingCalculatorLib.LightingProfile lightingProfile)<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void SetPlayerStart(System.Numerics.Vector3 position)<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void KillRunningArx()<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void StartArxFatalis(Boolean noClip, Boolean killRunningArx)<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void StartArxFatalis(Int32 levelId, Boolean noClip, Boolean killRunningArx)<br>
&nbsp;&nbsp;&nbsp;&nbsp;static Void PrintPsDocs()<br>
<br>
Additionally you may change the WellDoneArxLevel of the ScriptFunc class however you like. You might need to access classes from ArxLibertatisEditorIO for this<br>