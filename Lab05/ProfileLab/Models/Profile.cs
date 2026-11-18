namespace ProfileLab.Models;

// Domain state: what the app is for. Knows nothing about screens.
public record Profile(string Name, string Email);
