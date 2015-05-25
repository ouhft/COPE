from django.test import TestCase
from django.contrib.auth.models import User

from .models import Person


class PersonTests(TestCase):
    def test_superuser(self):
        superuser = User.objects.create_superuser('admin','test@test.com','passw0rd')
        self.assertEqual(superuser.id > 0, True, msg="No Superuser created")

        # now check we can find it in the database again
        all_users_in_database = User.objects.all()
        self.assertEquals(len(all_users_in_database), 1)
        only_user_in_database = all_users_in_database[0]
        self.assertEquals(only_user_in_database, superuser)
        superuser_by_username = User.objects.get_by_natural_key('admin')
        self.assertEquals(superuser_by_username, superuser, msg="No Superuser found")
        self.assertEqual(superuser.id, 1)

    # def test_full_name(self):
    #     """
    #     full_name() should return True for Persons if the first name is used first in the string.
    #     """
        test_first = 'test'
        test_last = 'case'
        test_job = 'SA'
        test_phone = '0123456789012345678901234'
        test_person = Person(
            first_names=test_first,
            last_names=test_last,
            job=test_job,
            telephone=test_phone,
            created_by=superuser
        )
        self.assertEqual(test_person.full_name().split(' ')[0] == test_first, True)
        self.assertEqual(test_person.full_name().split(' ')[-1] == test_last, True)
        self.assertEqual(test_person.id, None)

        test_person.save()
        self.assertEqual(len(test_person.telephone), len(test_phone))
        self.assertEqual(test_person.id, 1)

        self.assertEquals(unicode(test_person), "test case : Sys-admin")
