using AdventOfCodeCore.Models.Days;

namespace Problems._2025;

public class Point3d(int x, int y, int z)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public double DistanceTo(Point3d point)
    {
        return Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2) + Math.Pow(point.Z - Z, 2));
    }

    public int ClusterId = -1;

    public override string ToString() => $"{X},{Y},{Z}";

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}

public class Day8 : Day
{
    public override int Year => 2025;
    public override int DayNumber => 8;

    protected override string Part1()
    {
        return Solution();
    }

    protected override string Part2()
    {
        return Solution2();
        HashSet<Point3d> points = [];
        foreach (var row in Input)
        {
            var pos = row.Split(',');
            points.Add(new Point3d(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2])));
        }


        var closest = GetClosestPairs(points.ToList());
        var last = closest.Last();
        return (last.p2.X * last.p1.X).ToString();
    }

    protected string Solution2()
    {
        HashSet<Point3d> points = [];
        foreach (var row in Input)
        {
            var pos = row.Split(',');
            points.Add(new Point3d(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2])));
        }

        Dictionary<int, HashSet<Point3d>> clusters = [];

        int clusterIndex = 0;
        HashSet<(Point3d, Point3d)> checkedPairs = [];
        (Point3d, Point3d, double)? lastPair = null;
        //var closest = GetClosestPairs(points.ToList());
        while (true)
        {
            if (IsTest)
                Log.Log("");
            CheckStop();

            var maybePair = GetClosestPair(points, checkedPairs);
            if (!maybePair.HasValue)
                break;
            var pair = maybePair!.Value;
            //var pair = closest[i];
            if (IsTest)
                Log.Log(pair.p2 + " and " + pair.p1);

            if (pair.p2.ClusterId == -1 && pair.p1.ClusterId == -1)
            {
                pair.p1.ClusterId = clusterIndex;
                pair.p2.ClusterId = clusterIndex;
                clusters[clusterIndex] = [pair.p1, pair.p2];
                if (IsTest)
                    Log.Log("To Cluster: " + clusterIndex + " Total: " + clusters[clusterIndex].Count);
                clusterIndex++;
                continue;
            }

            if (pair.p1.ClusterId == pair.p2.ClusterId)
                continue;

            if (pair.p1.ClusterId != -1 && pair.p2.ClusterId != -1)
            {
                int idToMove = pair.p1.ClusterId;
                int targetId = pair.p2.ClusterId;
                foreach (var p in clusters[idToMove])
                {
                    p.ClusterId = targetId;
                    clusters[targetId].Add(p);
                }

                clusters.Remove(idToMove);
            }
            else if (pair.p1.ClusterId == -1)
            {
                pair.p1.ClusterId = pair.p2.ClusterId;
                clusters[pair.p2.ClusterId].Add(pair.p1);
                if (IsTest)
                    Log.Log("To Cluster: " + pair.p2.ClusterId + " Total: " + clusters[pair.p2.ClusterId].Count);
            }
            else if (pair.p2.ClusterId == -1)
            {
                pair.p2.ClusterId = pair.p1.ClusterId;
                clusters[pair.p1.ClusterId].Add(pair.p2);
                if (IsTest)
                    Log.Log("To Cluster: " + pair.p1.ClusterId + " Total: " + clusters[pair.p1.ClusterId].Count);
            }
            if(!IsTest)
                Log.Log("...");
            lastPair = pair;
        }

        return (lastPair!.Value.Item1.X * lastPair.Value.Item2.X).ToString();
    }

    protected string Solution()
    {
        HashSet<Point3d> points = [];
        foreach (var row in Input)
        {
            var pos = row.Split(',');
            points.Add(new Point3d(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2])));
        }

        Dictionary<int, HashSet<Point3d>> clusters = [];

        int clusterIndex = 0;
        HashSet<(Point3d, Point3d)> checkedPairs = [];
        var closest = GetClosestPairs(points.ToList());
        for (int i = 0; i < (IsTest ? 10 : 1000); i++)
        {
            if (IsTest)
                Log.Log("");
            CheckStop();

            //var maybePair = GetClosestPair(points, checkedPairs);
            //var pair = maybePair!.Value;
            var pair = closest[i];
            if (IsTest)
                Log.Log(pair.p2 + " and " + pair.p1);

            if (pair.p2.ClusterId == -1 && pair.p1.ClusterId == -1)
            {
                pair.p1.ClusterId = clusterIndex;
                pair.p2.ClusterId = clusterIndex;
                clusters[clusterIndex] = [pair.p1, pair.p2];
                if (IsTest)
                    Log.Log("To Cluster: " + clusterIndex + " Total: " + clusters[clusterIndex].Count);
                clusterIndex++;
                continue;
            }

            if (pair.p1.ClusterId == pair.p2.ClusterId)
                continue;

            if (pair.p1.ClusterId != -1 && pair.p2.ClusterId != -1)
            {
                int idToMove = pair.p1.ClusterId;
                int targetId = pair.p2.ClusterId;
                foreach (var p in clusters[idToMove])
                {
                    p.ClusterId = targetId;
                    clusters[targetId].Add(p);
                }

                clusters.Remove(idToMove);
                continue;
            }

            if (pair.p1.ClusterId == -1)
            {
                pair.p1.ClusterId = pair.p2.ClusterId;
                clusters[pair.p2.ClusterId].Add(pair.p1);
                if (IsTest)
                    Log.Log("To Cluster: " + pair.p2.ClusterId + " Total: " + clusters[pair.p2.ClusterId].Count);
                continue;
            }

            if (pair.p2.ClusterId == -1)
            {
                pair.p2.ClusterId = pair.p1.ClusterId;
                clusters[pair.p1.ClusterId].Add(pair.p2);
                if (IsTest)
                    Log.Log("To Cluster: " + pair.p1.ClusterId + " Total: " + clusters[pair.p1.ClusterId].Count);
                continue;
            }
        }

        foreach (var p in points)
        {
            if (p.ClusterId != -1)
                continue;
            clusters[clusterIndex] = [p];
            clusterIndex++;
        }

        var sortedClusters = clusters.Values.ToList();
        sortedClusters.Sort(HashSetComparer);

        Log.Log(string.Join(", ", sortedClusters.Select(c => c.Count).ToArray()));

        return (sortedClusters[0].Count * sortedClusters[1].Count * sortedClusters[2].Count).ToString();
    }

    private List<(Point3d p1, Point3d p2, double distance)> GetClosestPairs(List<Point3d> points)
    {
        Dictionary<(int, int), double> pairs = [];
        for (int i = 0; i < points.Count; i++)
        {
            double closestDistance = double.MaxValue;
            int closestIndex = -1;

            for (int j = 0; j < points.Count; j++)
            {
                if (i == j)
                    continue;
                var testPair = (Math.Min(i, j), Math.Max(j, i));
                if (pairs.ContainsKey(testPair))
                    continue;

                var d = points[i].DistanceTo(points[j]);
                if (d > closestDistance)
                    continue;

                closestDistance = d;
                closestIndex = j;
            }

            var pair = (Math.Min(i, closestIndex), Math.Max(closestIndex, i));
            pairs.Add(pair, closestDistance);
        }

        var sortedPairs = pairs.Select(p =>
            (points[p.Key.Item1], points[p.Key.Item2], p.Value)).ToList();
        sortedPairs.Sort(PairComparer);
        return sortedPairs.Distinct().ToList();
    }

    private (Point3d p1, Point3d p2, double distance)? GetClosestPair(HashSet<Point3d> points,
        HashSet<(Point3d, Point3d)> checkedPairs)
    {
        double closestDistance = double.MaxValue;
        Point3d? closestP1 = null;
        Point3d? closestP2 = null;
        foreach (var p1 in points)
        {
            foreach (var p2 in points)
            {
                if (p1.ClusterId != -1 && p2.ClusterId != -1)
                    continue;
                if (p1 == p2 || checkedPairs.Contains((p1, p2)) || checkedPairs.Contains((p2, p1)))
                    continue;
                var d = p1.DistanceTo(p2);
                if (d <= closestDistance)
                {
                    closestDistance = d;
                    closestP2 = p2;
                    closestP1 = p1;
                }
            }
        }

        if (closestP2 == null || closestP1 == null)
            return null;

        checkedPairs.Add((closestP1, closestP2));
        return (closestP1, closestP2, closestDistance);
    }


    int PairComparer(
        (Point3d p1, Point3d p2, double distance) pair1,
        (Point3d p1, Point3d p2, double distance) pair2) => pair1.distance.CompareTo(pair2.distance);

    int HashSetComparer(HashSet<Point3d> x, HashSet<Point3d> y) => y.Count.CompareTo(x.Count);
}