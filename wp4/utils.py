#!/usr/bin/python
# coding: utf-8
from django.contrib.auth.decorators import user_passes_test


# Based on https://djangosnippets.org/snippets/1703/
def group_required(*group_names):
    """Requires user membership in at least one of the groups passed in."""
    def in_groups(u):
        if u.is_authenticated():
            if u.is_superuser or bool(u.groups.filter(name__in=group_names)):
                return True
        return False
    return user_passes_test(in_groups)


def job_required(*job_names):
    """Requires user profile to have at least one of the jobs passed in."""
    def in_jobs(u):
        if u.is_authenticated():
            # TODO: Check that the user has a profile. They should, but mistakes happen.
            if u.is_superuser or bool(u.profile.has_job(job_names)):
                return True
        return False
    return user_passes_test(in_jobs)
