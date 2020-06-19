using System;

public class UserObj
{
	private string displayName;
	private string email;
	private string title;
	private string mobile;
	private string phone;

	public UserObj()
	{
	}

	public string DisplayName { get => displayName; set => displayName = value; }
	public string Email { get => email; set => email = value; }
	public string Title { get => title; set => title = value; }
	public string Mobile { get => mobile; set => mobile = value; }
	public string Phone { get => phone; set => phone = value; }
}
