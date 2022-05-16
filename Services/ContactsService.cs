﻿using Domain;
using Repository;

namespace Services
{
    public class ContactsService
    {
        private Context context = new Context();

        public List<Contact> getAll(string belong)
        {
            return context.Contacts.Where(contact => contact.belongTo == belong).ToList();
        }

        public void add(Contact contact)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
        }

        public Contact? getDetails(string belongTo, string id)
        {
            return context.Contacts.Find(belongTo, id);
        }

        public void edit(Contact newContact)
        {
            var contact = context.Contacts.Find(newContact.belongTo, newContact.id);
            if (newContact.name != null)
            {
                contact.name = newContact.name;
            }
            if (newContact.server != null)
            {
                contact.server = newContact.server;
            }
            if (newContact.last != null)
            {
                contact.last = newContact.last;
            }
            if (newContact.lastdate != null)
            {
                contact.lastdate = newContact.lastdate;
            }
            context.SaveChanges();
        }

        public void delete(string belongTo, string id)
        {
            Contact contact = context.Contacts.Find(belongTo, id);
            if (contact != null)
            {
                context.Contacts.Remove(contact);
                context.SaveChanges();
            }
            else throw new Exception("Contact not found");
        }

    }
}