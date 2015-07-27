Function Scaffold-CsTemplate([String]$Template, [String]$Project, [String]$OutputPath)
{
    if ($Delete)
    {
        Delete-ProjectItem $Project "$OutputPath.cs"
        Return;
    }

    $ModelName = ([Regex] "(?=[A-Z])").Split($Model, 0, 0)[-1]
    $ModelName = $ModelName.SubString(0, 1).ToLower() + $ModelName.SubString(1)

    $ControllerNamespace = "MvcTemplate.Controllers"
    $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers"
    If ($Area) { $ControllerNamespace = "MvcTemplate.Controllers.$Area" }
    If ($Area) { $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers.$Area" }

    Add-ProjectItemViaTemplate `
        -OutputPath $OutputPath `
        -Template $Template `
        -Model @{ `
            ControllerTestNamespace = $ControllerTestsNamespace; `
            AreaRegistration = $Area + "AreaRegistration"; `
            ControllerNamespace = $ControllerNamespace; `
            ModelName = $ModelName; `

            Controller = $Controller + "Controller"; `
            IValidator = "I" + $Model + "Validator"; `
            IService = "I" + $Model + "Service"; `
            Validator = $Model + "Validator"; `
            Service = $Model + "Service"; `
            View = $Model + "View"; `
            Model = $Model; `
            Area = $Area; `
        } `
        -SuccessMessage "Added $Project\{0}." `
        -TemplateFolders $TemplateFolders `
        -Project $Project
}
Function Scaffold-AreaRegistration([String]$Template, [String]$Project, [String]$OutputPath)
{
    if (!$Delete)
    {
        $ControllerNamespace = "MvcTemplate.Controllers.$Area"
        $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers.$Area"

        Add-ProjectItemViaTemplate `
            -OutputPath $OutputPath `
            -Template $Template `
            -Model @{ `
                ControllerTestNamespace = $ControllerTestsNamespace; `
                AreaRegistration = $Area + "AreaRegistration"; `
                ControllerNamespace = $ControllerNamespace; `
                Area = $Area; `
            } `
            -SuccessMessage "Added $Project\{0}." `
            -TemplateFolders $TemplateFolders `
            -Project $Project
    }
}
Function Scaffold-CshtmlTemplate([String]$Template, [String]$Project, [String]$OutputPath)
{
    if ($Delete)
    {
        Delete-ProjectItem $Project "$OutputPath.cshtml"
        Return;
    }

    $HeaderTitle = $Controller
    if ($Area) { $HeaderTitle = $Area + $HeaderTitle }

    Add-ProjectItemViaTemplate `
        -OutputPath $OutputPath `
        -Template $Template `
        -Model @{ `
            View = $Model + "View"; `
            HeaderTitle = $HeaderTitle; `
        } `
        -SuccessMessage "Added $Project\{0}." `
        -TemplateFolders $TemplateFolders `
        -Project $Project
}

Function Scaffold-ObjectMappingTests([String]$Project, [String]$Tests)
{
    if (!$Delete)
    {
        $TestsClass = Get-ProjectType -Project $Project -Type $Tests
        $Models = Get-PluralizedWord $Model

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added model/view mapping tests to $Tests." `
            -Template "Members\ObjectMappingTests" `
            -TemplateFolders $TemplateFolders `
            -CodeClass $TestsClass `
            -Model @{ `
                View = $Model + "View"; `
                Models = $Models; `
                Model = $Model; `
            }
    }
}

Function Scaffold-ObjectCreation([String]$Project, [String]$Factory)
{
    if (!$Delete)
    {
        $FactoryClass = Get-ProjectType -Project $Project -Type $Factory

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added tests object creation functions to $Factory." `
            -Template "Members\ObjectCreation" `
            -TemplateFolders $TemplateFolders `
            -CodeClass $FactoryClass `
            -Model @{ `
                View = $Model + "View"; `
                Model = $Model; `
            }
    }
}

Function Scaffold-ObjectMapping([String]$Project, [String]$Mapper)
{
    if (!$Delete)
    {
        $MapperClass = Get-ProjectType -Project $Project -Type $Mapper
        $Models = Get-PluralizedWord $Model

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added model/view mapping to $Mapper." `
            -Template "Members\ObjectMapping" `
            -TemplateFolders $TemplateFolders `
            -CodeClass $MapperClass `
            -Model @{ `
                View = $Model + "View"; `
                Models= $Models; `
                Model = $Model; `
            }
    }
}

Function Scaffold-DbSet([String]$Project, [String]$Context)
{
    if (!$Delete)
    {
        $ContextClass = Get-ProjectType -Project $Project -Type $Context
        $Models = Get-PluralizedWord $Model

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added DbSet<$Model> member to $Context." `
            -TemplateFolders $TemplateFolders `
            -Template "Members\DbSet" `
            -CodeClass $ContextClass `
            -Model @{ `
                Models = $Models; `
                Model = $Model; `
            }
    }
}

Function Delete-AreaRegistration([String]$Project, [String]$AreaPath, [String]$Path)
{
    $AreaRegistrationDir = Get-ProjectItem -Project $Project -Path $AreaPath
    If ($AreaRegistrationDir)
    {
        $ObjectCount = (Get-ChildItem $AreaRegistrationDir.FileNames(0) | Measure-Object).Count
        If ($ObjectCount -eq 1)
        {
            Delete-ProjectItem $Project "$Path.cs"
            Delete-EmptyDir $Project $AreaPath
        }
    }
}
Function Delete-ProjectItem([String]$Project, [String]$Path)
{
    $ProjectItem = Get-ProjectItem -Project $Project -Path $Path
    If ($ProjectItem)
    {
        $ProjectItem.Delete()
        Write-Host "Deleted $Project\$Path"
    }
    Else
    {
        Write-Host "$Project\$Path is missing! Skipping..." -BackgroundColor Yellow;
    }
}

Function Delete-EmptyDir([String]$Project, [String]$Dir)
{
    $ProjectItem = Get-ProjectItem -Project $Project -Path $Dir
    If ($ProjectItem -and !(Get-ChildItem $ProjectItem.FileNames(0) -force))
    {
        $ProjectItem.Delete()
        Write-Host "Deleted $Project\$Dir"
    }
}
