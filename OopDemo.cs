using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        HeadOfHousehold mother = new HeadOfHousehold("Billy", "Jean", new DateTime(1983, 2, 1));
        Dependent child = new Dependent("Joe", "Jean", new DateTime(2001, 6, 1), Relationship.Child);
        Dependent child2 = new Dependent("Bob", "Jean", new DateTime(2003, 5, 24), Relationship.Child);
        mother.Add(child);
        mother.Add(child2);
        TaxHouseHold household = new TaxHouseHold();
        household.Add(mother);
        household.Show();
        bool result = IsAwesome(mother); 
        Console.WriteLine($"Is Awesome = {result}");
        Console.ReadLine(); //Wait for someone to press enter
    }

    static bool IsAwesome(IPerson p)
    {
        return p.FirstName.StartsWith("B");
    }
}

public interface IPerson
{
    DateTime BirthDate { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    void Show();
}

public class Person : IPerson
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    private DateTime _birthDate = DateTime.MinValue;

    public Person(string firstName, string lastName, DateTime birthDate)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.BirthDate = birthDate;
    }

    public DateTime BirthDate
    {
        get { return _birthDate; }
        set { _birthDate = new DateTime(value.Year, value.Month, value.Day); }
    }

    public virtual void Show()
    {
        Console.WriteLine($"{FirstName} {LastName} {BirthDate.ToShortDateString()}");
    }

    protected int Age(DateTime dob)
    {
        return new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
    }
}

public enum Relationship
{
    None,
    Spouse,
    Child,
    GrandChild,
}

public class Dependent : Person
{
    public Dependent(string firstName, string lastName, DateTime birthDate, Relationship relation)
        : base(firstName, lastName, birthDate)
    {
        this.Relation = relation;
    }

    public Relationship Relation { get; private set; }

    public override void Show()
    {
        Console.WriteLine($"  {Relation} {FirstName} {LastName} Age: {Age(BirthDate)}");
    }
}

public class HeadOfHousehold : Person
{
    public List<Dependent> dependents { get; private set; }

    public HeadOfHousehold(string firstName, string lastName, DateTime birthDate)
        : base(firstName, lastName, birthDate)
    {
        dependents = new List<Dependent>();
    }

    public void Add(Dependent dependent)
    {
        dependents.Add(dependent);
    }
}


public abstract class People
{
    protected List<Person> people;

    public People()
    {
        people = new List<Person>();
    }

    public Person Find(Type type)
    {
        for (int j = people.Count - 1; j >= 0; j--)
        {
            if (type == people[j].GetType())
            {
                return people[j];
            }
        }
        return null;
    }

    public virtual void Add(Person person)
    {
        people.Add(person);
    }

    public virtual void Show()
    {
        foreach (Person p in people)
        {
            p.Show();
        }
    }
}

public class TaxHouseHold : People
{
    public override void Add(Person person)
    {
        if (person is HeadOfHousehold)
        {
            base.Add(person);
        }
        else
        {
            HeadOfHousehold head = Find(typeof(HeadOfHousehold)) as HeadOfHousehold;
            if (head != null)
            {
                base.Add(person);
            }
        }
    }

    public override void Show()
    {
        foreach (Person p in people)
        {
            if (p is HeadOfHousehold)
            {
                p.Show();

                HeadOfHousehold head = p as HeadOfHousehold;
                foreach (Dependent d in head.dependents)
                {
                    d.Show();
                }
            }
        }
    }
}

public class HouseHold : People
{
    public override void Add(Person person)
    {
        base.Add(person);
    }
}
