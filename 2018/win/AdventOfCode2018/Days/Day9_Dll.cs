// Based on Zamansky's Java Dll for Advent of Code 2018 here:
// https://github.com/zamansky/advent2018/tree/master/java

using System;

public class Program
{
	public static void Main()
	{
		Dll l = new Dll();
		int i, lastscore=7095000, numplayers=431, player=0;
		long[] players = new long[numplayers];
        for (i=3;i<=lastscore;i++)
		{
	    	if (i%23!=0)
			{
				l.inc();
				l.insert(i);
	    	} else {
				players[player]+=i;
				for (int j = 0; j < 7; j++)
				{
		    		l.dec();
				}
				players[player]+=l.getCurrent();
                l.delete();
	    	}
	    	player = (player+1)%numplayers;
		}
		
	Array.Sort(players);
	Console.WriteLine(players[numplayers-1]);
	}
}

public class Node
{
    public int data;
    public Node next,prev;
    public Node(int d)
	{
		data = d;
		next=null;
		prev=null;
    }
    public void setNext(Node n){next = n;}
    public void setPrev(Node n){prev = n;}
    public int getData(){return data;}
    public Node getNext(){return next;}
    public Node getPrev(){return prev;}
}

public class Dll
{
    Node current;
    
	public Dll()
	{
		current = null;
		Node n = new Node(0);
		Node n2 = new Node(1);
		Node n3 = new Node(2);
		current = n3;
		n3.setNext(n2);n3.setPrev(n);
		n2.setNext(n);n2.setPrev(n3);
		n.setNext(n3);n.setPrev(n2);
    }

    public String toString()
	{
		String s = "";
		Node t = current.getNext();
		while (t!=current)
		{
	    	s=s+t.getData()+" ";
	    	t=t.getNext();
		}
		
		s=s+t.getData();
		return s;
    }

    public void inc()
	{
		current = current.getNext();
	}
	
    public void dec()
	{
		current = current.getPrev();
	}
	
    public int getCurrent()
	{
		return current.getData();
	}

    public void insert(int d)
	{
		Node n = new Node(d);
		n.setNext(current.getNext());
		n.setPrev(current);
		n.getNext().setPrev(n);
		n.getPrev().setNext(n);
		current = n;
    }
	
    public void delete()
	{
		Node tmp=current;
		current=current.getNext();
		tmp.getNext().setPrev(tmp.getPrev());
		tmp.getPrev().setNext(tmp.getNext());  
    }
}