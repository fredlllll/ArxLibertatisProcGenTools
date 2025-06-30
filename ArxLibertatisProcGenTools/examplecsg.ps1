#this is an example on how to utilize the procgen tools from a powershell script
#namespaces apparently have to come before anything in the file, even if they dont exist at that point
using namespace ArxLibertatisProcGenTools.Generators.Light
using namespace ArxLibertatisProcGenTools.Generators.Plane
using namespace ArxLibertatisProcGenTools.Generators.Texture
using namespace ArxLibertatisProcGenTools.Generators.Mesh
using namespace ArxLibertatisProcGenTools.Modifiers
using namespace ArxLibertatisProcGenTools.Shapes
using namespace ArxLibertatisProcGenTools.Values
using namespace ArxLibertatisEditorIO.Util
using namespace System.Numerics
using namespace Csg
# Require PowerShell 7 or newer cause net standard 2.1 is required
if ($PSVersionTable.PSVersion.Major -lt 7) {
    Write-Warning "This script needs PowerShell 7+. Current version: $($PSVersionTable.PSVersion). Get it here https://github.com/PowerShell/PowerShell/releases/tag/v7.5.1"
    exit 1
}
Set-StrictMode -Version 3.0
# import the library
Add-Type -Path "ArxLibertatisProcGenTools.dll"
Add-Type -Path "DotNetCsg.dll"

# set the data dir containing the unpacked game and graph directories
[ArxLibertatisProcGenTools.ScriptFunc]::SetDataDir("F:\Program Files\Arx Libertatis")

# clear in memory level as it is kept for the duration of the powershell session
[ArxLibertatisProcGenTools.ScriptFunc]::Clear()

# add a floor
$floor = New-Object FloorGenerator
$floor.Size = New-Object Vector2(5000, 5000)
$floor.Center = New-Object Vector3(6000, -100, 6000)
$floor.TextureGenerator = New-Object SingleTexture("graph\obj3d\textures\[soil]_human_standard2.jpg")
[ArxLibertatisProcGenTools.ScriptFunc]::Apply($floor)

# add more polygon detail in the center
$enhance = New-Object DetailEnhancer
$shape = New-Object Sphere
$shape.Radius = 500
$shape.Falloff = 0
$enhance.Shape = $shape;
[ArxLibertatisProcGenTools.ScriptFunc]::Apply($enhance)

# add some noise(rumble) to the existing polygons with a falloff spherical shape
$rumble = New-Object Rumble
$noise = New-Object SimplexNoiseValue
$noise.Noise.Frequency = 0.002
$noise.Noise.OctaveCount = 2
$rumble.NoiseValue = $noise
$rumble.Magnitude = 100

$shape = New-Object Sphere
$shape.Center = New-Object Vector3(6000,0,6000)
$shape.Radius = 2000
$shape.Falloff = 1000

$vector = New-Object FixedVector
$vector.Value = New-Object Vector3(0.1,1,0.1)

$rumble.Shape = [ArxLibertatisProcGenTools.Shapes.IShapeExtensions]::MultiplyWith($shape,$vector)
[ArxLibertatisProcGenTools.ScriptFunc]::Apply($rumble)

#add a light
$light = New-Object ArxLibertatisEditorIO.WellDoneIO.Light
$light.position = New-Object Vector3(6000,-400,6000)
$light.color = New-Object Color(1,0.7,0.7)
$light.fallStart = 100
$light.fallEnd = 1000
$light.intensity = 1
[ArxLibertatisProcGenTools.ScriptFunc]::Level.lights.Add($light)

#create the CSG Solid
$rod = [Solids]::Cylinder(0.5, 2, $true)
$solid = [Solids]::Difference(
    [Solids]::Intersection(
        [Solids]::Cube(1.4, $true),
        [Solids]::Sphere(1, $true)
    ),
    [Solids]::Union(
        $rod,
        $rod.RotateX(90),
        $rod.RotateZ(90)
    )
)

$csg = [CSGGenerator]::new()
$csg.CsgSolid = $solid
$csg.PositionOffset = [Vector3]::new(6500,-150,6000)
$csg.Scale = [Vector3]::new(100,100,100)
#polytype glow will make the resulting mesh always visible at 100% brightess. remove this line to just have normal graphics
$csg.PolyType = [PolyType]::GLOW
$csg.TextureGenerator = $floor.TextureGenerator
[ArxLibertatisProcGenTools.ScriptFunc]::Apply($csg)

#set lighting profile
[ArxLibertatisProcGenTools.ScriptFunc]::SetLightingProfile([ArxLibertatisLightingCalculatorLib.LightingProfile]::DistanceAngleShadow)

#set player starting position
$pos = New-Object Vector3(6000,-500,6000)
[ArxLibertatisProcGenTools.ScriptFunc]::SetPlayerStart($pos)

#save level
[ArxLibertatisProcGenTools.ScriptFunc]::SaveLevel("level1")