using System.Collections.Generic;

public class Spells
{
  public enum SpellSymbols
  {
    Neutral,
    Push,
    Pull,
    Right,
    Left,
    Lift,
    Drop,
    None
  }
  public enum SpellElement
  {
    Fire,
    Ice,
    Earth,
    Wind,
    Poison,
    Psychic,
  }
  public enum SpellType
  {
    Arrow,
    Ball,
    Aura,
    Telekinesis,
    Flight
  }

  public static Dictionary<string, List<SpellSymbols>> Formula = new Dictionary<string, List<SpellSymbols>>()
  {
    {"Fire Arrow", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Right, SpellSymbols.Left, SpellSymbols.Push}},
    {"Fire Ball", new List<SpellSymbols>{SpellSymbols.Push, SpellSymbols.Right, SpellSymbols.Left, SpellSymbols.Left}},
    {"Ice Arrow", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Left, SpellSymbols.Right, SpellSymbols.Push}},
    {"Ice Ball", new List<SpellSymbols>{SpellSymbols.Right, SpellSymbols.Left, SpellSymbols.Right, SpellSymbols.Pull}},
    {"Poison Arrow", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Right, SpellSymbols.Push}},
    {"Poison Ball", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Left, SpellSymbols.Push, SpellSymbols.Push}},
    {"Earth Arrow", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Left, SpellSymbols.Push}},
    {"Earth Ball", new List<SpellSymbols>{SpellSymbols.Pull, SpellSymbols.Left, SpellSymbols.Pull, SpellSymbols.Push}},
    {"Telekinesis", new List<SpellSymbols>{SpellSymbols.Push, SpellSymbols.Pull, SpellSymbols.Left, SpellSymbols.Right, SpellSymbols.Pull, SpellSymbols.Pull}},
    {"Flight", new List<SpellSymbols>{SpellSymbols.Push, SpellSymbols.Pull, SpellSymbols.Right, SpellSymbols.Left, SpellSymbols.Push, SpellSymbols.Push}},
  };

  public static Dictionary<string, SpellEffect> SpellEffectDictionary = new Dictionary<string, SpellEffect>()
  {
    {"Fire Arrow", new SpellEffect(SpellElement.Fire, 0.5f, SpellType.Arrow)},
    {"Fire Ball",  new SpellEffect(SpellElement.Fire, 2f, SpellType.Ball)},
    {"Ice Arrow", new SpellEffect(SpellElement.Ice, 0.5f, SpellType.Arrow)},
    {"Ice Ball", new SpellEffect(SpellElement.Ice, 5f, SpellType.Ball)},
    {"Poison Arrow", new SpellEffect(SpellElement.Poison, 0.5f, SpellType.Arrow)},
    {"Poison Ball", new SpellEffect(SpellElement.Poison, 1f, SpellType.Ball)},
    {"Earth Arrow", new SpellEffect(SpellElement.Fire, 0.5f, SpellType.Arrow)},
    {"Earth Ball", new SpellEffect(SpellElement.Fire, 0.5f, SpellType.Ball)},
    {"Telekinesis", new SpellEffect(SpellElement.Psychic, 0, SpellType.Telekinesis)},
    {"Flight", new SpellEffect(SpellElement.Psychic, 0, SpellType.Flight)},
  };

  public class SpellEffect
  {
    public SpellElement element;
    public float cooldown;
    public SpellType type;
    public SpellEffect(SpellElement _element, float _cooldown, SpellType _type)
    {
      element = _element;
      cooldown = _cooldown;
      type = _type;
    }
  }
}
