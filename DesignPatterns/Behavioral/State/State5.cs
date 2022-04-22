using Stateless;
using System.Text;

namespace DesignPatterns.Behavioral.State;

public static class State5
{
    public enum Health
    {
        NonReprodutive,
        Pregnant,
        Reproductive
    }

    public enum Activity
    {
        GiveBirth,
        ReachPuberty,
        HaveAbortion,
        HabeUnprotectedSex,
        Historectmy
    }

    public static void Render()
    {
        var machine = new StateMachine<Health, Activity>(Health.NonReprodutive);
        machine.Configure(Health.NonReprodutive).Permit(Activity.ReachPuberty, Health.Reproductive);
    }
}
