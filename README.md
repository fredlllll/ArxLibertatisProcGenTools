this is a library that is intended to either be referenced from nuget, or imported in powershell scripts (needs powershell 7+)

you can modify or generate levels for arx fatalis with it, programatically

there is an example script called example.ps1

run docs.ps1 to display the following documentation
 
```
Powershell Documentation:
This is a listing of all relevant classes and functions for generating levels in powershell scripts
Mesh Generators are classes that will generate polygons that will be added to the level. These are available:
ArxLibertatisProcGenTools.Generators.Plane.FloorGenerator
Generates a flat surface of a certain size around a center, using a texture generator
  Constructors:
    FloorGenerator()
  Properties:
    ITextureGenerator TextureGenerator
    Vector2 Size
    Vector3 Center
  Methods:
    IEnumerable`1 GetPolygons()

ArxLibertatisProcGenTools.Generators.Plane.QuadGenerator
Generates a single quad in a certain orientation
  Constructors:
    QuadGenerator()
  Properties:
    Vector3 Center
    Single Width
    Single Height
    Vector3 Normal
    Vector3 WorldUp
    Single MinU
    Single MaxU
    Single MinV
    Single MaxV
    Int16 Room
    PolyType PolyType
    String TexturePath
    Single TransVal
  Methods:
    IEnumerable`1 GetPolygons()
    Polygon GetPolygon()

ArxLibertatisProcGenTools.Generators.Mesh.OBJImporter
Imports obj files, only supports triangulated files. set Pos/Rot/Scale using WorldMatrix
  Constructors:
    OBJImporter(System.String objFilePath, System.String mtlFilePath)
  Properties:
    Matrix4x4 WorldMatrix
    Int16 Room
  Methods:
    IEnumerable`1 GetPolygons()

Light generators generate lights that will be added to the level. These are available:
ArxLibertatisProcGenTools.Generators.Light.RandomLightGenerator
Generates lights at random positions within its shape, with random attributes dictated by their shapes
  Constructors:
    RandomLightGenerator()
  Properties:
    Int32 Count
    IShape PositionShape
    IShape ColorShape
    IValue FalloffStart
    IValue FalloffEnd
    IValue Intensity
  Methods:
    IEnumerable`1 GetLights()

Texture generators generate texture names depending on position. These are available:
ArxLibertatisProcGenTools.Generators.Texture.SingleTexture
Returns just a fixed texture
  Constructors:
    SingleTexture(System.String path)
  Properties:
  Methods:
    String GetTexturePath(Int32 polygonIndex)

Modifiers modify the currently existing polygons. These are available:
ArxLibertatisProcGenTools.Modifiers.DetailEnhancer
Generates extra polygons in an area so it can be sculpted to a greater detail by modifiers
  Constructors:
    DetailEnhancer()
  Properties:
    IShape Shape
  Methods:
    Void Apply(ArxLibertatisEditorIO.WellDoneIO.WellDoneArxLevel wdl)

ArxLibertatisProcGenTools.Modifiers.Rumble
Adds random offsets(noise) to polygons in an area
  Constructors:
    Rumble()
  Properties:
    Single Magnitude
    IShape Shape
    IValue NoiseValue
  Methods:
    Void Apply(ArxLibertatisEditorIO.WellDoneIO.WellDoneArxLevel wdl)

Shapes can be used in a lot of ways to shape the output of other classes. These are available:
ArxLibertatisProcGenTools.Shapes.Cuboid
The shape of a cube
  Constructors:
    Cuboid()
  Properties:
    Vector3 Min
    Vector3 Max
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

ArxLibertatisProcGenTools.Shapes.Everywhere
  Constructors:
    Everywhere()
  Properties:
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

ArxLibertatisProcGenTools.Shapes.FixedVector
Returns a fixed value
  Constructors:
    FixedVector()
    FixedVector(System.Numerics.Vector3 value)
  Properties:
    Vector3 Value
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

ArxLibertatisProcGenTools.Shapes.NullShape
Always returns zero
  Constructors:
    NullShape()
  Properties:
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

ArxLibertatisProcGenTools.Shapes.MultiplyShape
returns the multiplication of two shapes
  Constructors:
    MultiplyShape()
  Properties:
    IShape Shape1
    IShape Shape2
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

ArxLibertatisProcGenTools.Shapes.Sphere
the shape of a sphere
  Constructors:
    Sphere()
  Properties:
    Vector3 Center
    Single Radius
    Single Falloff
      the falloff area around the edge of the sphere (r:500, f:200 => falloff from 400 to 600)
  Methods:
    Vector3 GetAffectedness(System.Numerics.Vector3 position)
    Vector3 GetRandomPosition()

Values are similar to Shapes, just one dimensional. These are available:
ArxLibertatisProcGenTools.Values.FixedValue
A fixed value
  Constructors:
    FixedValue()
    FixedValue(Single value)
    FixedValue(Double value)
  Properties:
    Single Value
  Methods:
    Single GetValue(System.Numerics.Vector3 input)

ArxLibertatisProcGenTools.Values.NullValue
Always returns zero
  Constructors:
    NullValue()
  Properties:
  Methods:
    Single GetValue(System.Numerics.Vector3 input)

ArxLibertatisProcGenTools.Values.RandomValue
random value (uniform)
  Constructors:
    RandomValue()
  Properties:
    Single Min
    Single Max
  Methods:
    Single GetValue(System.Numerics.Vector3 input)

ArxLibertatisProcGenTools.Values.SimplexNoiseValue
random value (simplex, like perlin but better), get Noise property to change parameters
  Constructors:
    SimplexNoiseValue()
  Properties:
    Simplex Noise
  Methods:
    Single GetValue(System.Numerics.Vector3 input)

In addition to these classes, there is the ScriptFunc class, which contains static functions that make scripting easier:
  Properties:
    static WellDoneArxLevel Level
  Methods:
    static Void Clear()
      deletes the level currently in memory. required cause its persistent during the powershell session
    static Void SetDataDir(System.String dataDir)
      Set data dir that the level is saved to (and can be loaded from)
    static Void LoadLevel(System.String name)
      Loads a level to be modified
    static Void SaveLevel(System.String name)
      Saves the level
    static Void Apply(ArxLibertatisProcGenTools.Generators.IMeshGenerator meshGenerator)
      Applies the Mesh Generator to the level
    static Void Apply(ArxLibertatisProcGenTools.Modifiers.IModifier meshModifier)
      Applies the Modifier to the level
    static Void Apply(ArxLibertatisProcGenTools.Generators.ILightGenerator lightGenerator)
      Applies the Light Generator to the level
    static Void SetLightingProfile(ArxLibertatisLightingCalculatorLib.LightingProfile lightingProfile)
    static Void SetPlayerStart(System.Numerics.Vector3 position)
    static Void PrintPsDocs()

Additionally you may change the WellDoneArxLevel of the ScriptFunc class however you like. You might need to access classes from ArxLibertatisEditorIO for this
```
