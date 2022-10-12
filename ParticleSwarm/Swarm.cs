using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSwarm
{
    public struct Particle
    {
        public double x;
        public double xeval;
        public double b;
        public double bg;
        public double v;
    }
    public struct Result
    {
        public double fmax;
        public double fmin;
        public double favg;
    }
    class Swarm
    {
        public List<Result> results;

        public List<List<Double>> history;
        public List<Particle> InitializeSwarm(int a, int b, int N, int decimals)
        {
            Random rand = new Random();
            List<Particle> swarm = new List<Particle>();
            history = new List<List<double>>();
            for (int i = 0; i < N; i++)
            {
                history.Add(new List<double>());
                var x = new Particle();
                var temp = Math.Round(rand.NextDouble() * (b - a) + a, decimals);
                x.x = temp;
                x.b = temp;
                x.bg = temp;
                x.xeval = Feval(temp);
                x.v = 0;
                swarm.Add(x);
            }
            return swarm;
        }
        public List<Particle> SimulateSwarm(List<Particle> Swarm, int N, int r, double c1, double c2, double c3, int T, int decimals)
        {
            results = new List<Result>();
            for (int i = 0; i < T; i++)
            {
                Swarm = SingleIterration(Swarm, N, r, c1, c2, c3, decimals);
                var result = new Result();
                result.favg = Swarm.Average(a => a.xeval);
                result.fmax = Swarm.Max(a => a.xeval);
                result.fmin = Swarm.Min(a => a.xeval);
                results.Add(result);
            }
            return Swarm;
        
        }
        public List<Particle> SingleIterration(List<Particle> Swarm, int N, int r, double c1, double c2, double c3, int decimals)
        {
            for (int i = 0; i < Swarm.Count; i++)
            {
                var temp = Swarm[i];
                history[i].Add(temp.x);
                temp.xeval = Feval(temp.x);
                if (Feval(temp.b) < Feval(temp.x)) temp.b = temp.x;
                if (Feval(temp.bg) < Feval(temp.x)) temp.bg = temp.x;
                Swarm[i] = temp;
            }
            for (int i = 0; i < Swarm.Count; i++)
            {
                var temp = FindNeighbours(Swarm, N, r, Swarm[i].x);
                var bestNeighbour = temp.OrderByDescending(a => Feval(a.bg)).First();
                if (Swarm[i].bg < bestNeighbour.bg)
                {
                    var change = Swarm[i];
                    change.bg = bestNeighbour.bg;
                    Swarm[i] = change;
                }
            }
            Random rand = new Random();
            for (int i = 0; i < Swarm.Count; i++)
            {
                var temp = Swarm[i];
                temp.v = (temp.v * c1 * rand.NextDouble()) + (c2 * rand.NextDouble() * (temp.b - temp.x)) + (c3* rand.NextDouble() * (temp.bg - temp.x));
                temp.x += temp.v;
                temp.x = Math.Round(temp.x, decimals);
                Swarm[i] = temp;
            }
            return Swarm;

        }
        public double Feval(double xreal)
        {
            return (xreal - (Math.Truncate(xreal))) * (Math.Cos(20 * Math.PI * xreal) - Math.Sin(xreal));
        }
        public List<Particle> FindNeighbours(List<Particle> Swarm, int N, int r, double x)
        {
            int amount = (int)Math.Ceiling((double)((double)N * ((double)r / (double)100)));
            var temp = Swarm.OrderBy(a => Math.Abs(a.x - x)).Take(amount).ToList();
            return temp.OrderByDescending(a => Feval(a.bg)).ToList(); 
        }
    }
}
