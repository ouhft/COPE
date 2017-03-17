#!/usr/bin/python
# coding: utf-8

"""
Utility methods to help setup the test environment
"""

def get_permission_dictionary():
    """
    Build a dictionary of all permissions, ordered by their content type

    :return: dict
    """
    from django.contrib.auth.models import Permission
    from django.contrib.contenttypes.models import ContentType

    result = {}
    for content_type in ContentType.objects.all():
        ctype_label = "{0}__{1}".format(content_type.app_label, content_type.model)
        result[ctype_label] = {
            "id": content_type.id,
            "plist": {}
        }

    for permission in Permission.objects.all().order_by('content_type'):
        ctype_label = "{0}__{1}".format(permission.content_type.app_label, permission.content_type.model)
        result[ctype_label]["plist"][permission.codename] = permission

    # print("get_permission_dictionary()={0}".format(result))
    return result

#  HELP SCRIPT to generate the test/base content for the migration
# from django.contrib.auth.models import Group
# for group in Group.objects.all():
#     print("===========  {0}  ===================================".format(group.name))
#     for perm in group.permissions.all():
#         print("group.permissions.add(pd[\"{0}__{1}\"][\"plist\"][\"{2}\"])".format(
#             perm.content_type.app_label,
#             perm.content_type.model,
#             perm.codename
#         ))

