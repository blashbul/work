# Dynamic & linq

So long as data is an IEnumerable of some kind, you can use:

var a = ((IEnumerable) data).Cast<dynamic>().Where(p => p.verified);

The Cast<dynamic>() is to end up with an IEnumerable<dynamic> so that the type of the parameter to the lambda expression is also dynamic.

À partir de l’adresse <http://stackoverflow.com/questions/18734996/how-to-use-linq-with-dynamic-collections> 
