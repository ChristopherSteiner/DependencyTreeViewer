using DependencyTreeViewer.Application.Extensions;
using DependencyTreeViewer.Application.NugetTree;
using Snapshooter.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace DependencyTreeViewer.UI.Tests.Extensions;

public class ListExtensionsTest
{
    private readonly List<ProjectModel> projects = new List<ProjectModel>();

    public ListExtensionsTest(ITestOutputHelper testOutput)
    {
        projects = new List<ProjectModel>
        {
            new ProjectModel
            {
                Name = "Foo.csproj",
                TargetFrameworks = new List<FrameworkModel>
                {
                    new FrameworkModel
                    {
                        Name = "net8.0",
                        References = new List<PackageModel>
                        {
                            new PackageModel
                            {
                                Name = "Some.Library",
                                Version = "1.0.0",
                                References = new List<PackageModel>
                                {
                                    new PackageModel
                                    {
                                        Name = "Some.Library.Abc",
                                        Version = "1.3.0",
                                        References = new List<PackageModel>
                                        {
                                            new PackageModel { Name = "Some.Library.Abc.Abstractions", Version = "1.4.1" },
                                            new PackageModel { Name = "Some.Library.Cba", Version = "1.5.3" }
                                        }
                                    },
                                    new PackageModel
                                    {
                                        Name = "Some.Library.Def",
                                        Version = "2.3.1",
                                        References = new List<PackageModel>
                                        {
                                            new PackageModel { Name = "Some.Library.Def.Abstractions", Version = "1.1.0" }
                                        }
                                    }
                                }
                            },
                            new PackageModel
                            {
                                Name = "Other.Library",
                                Version = "2.3.1"
                            },
                            new PackageModel
                            {
                                Name = "Default.Class",
                                Version = "3.0.0",
                                References = new List<PackageModel>
                                {
                                    new PackageModel { Name = "Default.Class.Abstractions", Version = "3.0.0" }
                                }
                            }
                        }
                    }
                }
            },
            new ProjectModel
            {
                Name = "Bar.csproj",
                TargetFrameworks = new List<FrameworkModel>
                {
                    new FrameworkModel
                    {
                        Name = "net8.0",
                        References = new List<PackageModel>
                        {
                            new PackageModel
                            {
                                Name = "Other.Library",
                                Version = "4.3.1"
                            },
                            new PackageModel
                            {
                                Name = "Unit.Library",
                                Version = "18.8.6",
                                References = new List<PackageModel>
                                {
                                    new PackageModel { Name = "Test.Library", Version = "12.1.2" },
                                    new PackageModel
                                    {
                                        Name = "Default.Class",
                                        Version = "3.0.0",
                                        References = new List<PackageModel>
                                        {
                                            new PackageModel { Name = "Default.Class.Abstractions", Version = "3.0.0" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    public void Verify_Search_HitOnLastNoteInTree()
    {
        List<ProjectModel> result = projects.Search("cba").ToList();

        Snapshot.Match(result);
    }

    [Fact]
    public void Verify_Search_HitSomewhereInTheMiddleOfTheTree()
    {
        List<ProjectModel> result = projects.Search("Unit").ToList();

        Snapshot.Match(result);
    }

    [Fact]
    public void Verify_Search_MultipleHits()
    {
        List<ProjectModel> result = projects.Search("Library").ToList();

        Snapshot.Match(result);
    }

}
