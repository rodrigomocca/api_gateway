using System.Threading.Tasks;

namespace ApiLauncher.Models
{
    public interface IExecutor
    {
        void Launch(Microservice microservice, string filename = "");

        bool Initialized();

        Task Cancel(string name);
    }
}