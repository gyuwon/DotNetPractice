struct Contact {
  1: i32 id,
  2: required string firstName,
  3: required string lastName,
  4: required string email
}

service ContactsService {
  list<Contact> getContacts(),
  list<Contact> addContacts(1: list<Contact> contacts)
}
