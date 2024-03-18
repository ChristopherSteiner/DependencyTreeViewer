using DependencyTreeViewer.Application.NugetTree;

namespace DependencyTreeViewer.Application.Extensions;

public static class ListExtensions
{
    public static void AddUniqueWithLimit<T>(this List<T> list, T item, int limit)
    {
        if (list.Contains(item))
        {
            list.Remove(item);
            list.Add(item);
        }
        else
        {
            if (list.Count >= limit)
            {
                list.RemoveAt(0);
            }

            list.Add(item);
        }
    }

    public static List<T> Limit<T>(this List<T> list, int limit)
    {
        int limitDifference = list.Count - limit;

        if (limitDifference > 0)
        {
            list.RemoveRange(0, limitDifference);
        }

        return list;
    }

    public static List<ProjectModel> Clone(this IEnumerable<ProjectModel> projects)
    {
        List<ProjectModel> clonedProjects = new();

        foreach (ProjectModel project in projects)
        {
            ProjectModel clonedProject = new();
            clonedProject.Name = project.Name;
            clonedProject.TargetFrameworks = Clone(project.TargetFrameworks);

            clonedProjects.Add(clonedProject);
        }

        return clonedProjects;
    }

    public static IEnumerable<ProjectModel> Search(this IEnumerable<ProjectModel> projects, string searchString)
    {
        foreach (ProjectModel project in projects)
        {
            foreach (FrameworkModel frameworkModel in project.TargetFrameworks)
            {
                frameworkModel.References = frameworkModel.References.Search(searchString);
            }
        }

        return projects;
    }

    private static List<FrameworkModel> Clone(IEnumerable<FrameworkModel> frameworks)
    {
        List<FrameworkModel> clonedFrameworks = new();

        foreach (FrameworkModel framework in frameworks)
        {
            FrameworkModel clonedFramework = new();
            clonedFramework.Name = framework.Name;
            clonedFramework.References = Clone(framework.References);

            clonedFrameworks.Add(clonedFramework);
        }

        return clonedFrameworks;

    }

    private static List<PackageModel> Clone(IEnumerable<PackageModel> packages)
    {
        List<PackageModel> clonedPackages = new();

        foreach (PackageModel package in packages)
        {
            PackageModel clonedPackage = new();
            clonedPackage.Name = package.Name;
            clonedPackage.ReferencedVersion = package.ReferencedVersion;
            clonedPackage.ActualVersion = package.ActualVersion;
            clonedPackage.References = Clone(package.References);

            clonedPackages.Add(clonedPackage);
        }

        return clonedPackages;
    }

    private static IEnumerable<PackageModel> Search(this IEnumerable<PackageModel> packages, string searchString)
    {
        foreach (PackageModel package in packages)
        {
            if (package.References.Any())
            {
                IEnumerable<PackageModel> foundPackages = package.References.Search(searchString);
                if (foundPackages.Any())
                {
                    package.References = foundPackages;
                    yield return package;
                }
                else if (package.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
                {
                    package.References = Enumerable.Empty<PackageModel>();
                    yield return package;
                }
            }
            else if (package.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return package;
            }
        }
    }
}
