#!/usr/bin/python
# coding: utf-8

from django.views.generic import ListView, CreateView, UpdateView, DetailView

from .models import StaffPerson


class StaffPersonListlView(ListView):
    model = StaffPerson


class StaffPersonDetailView(DetailView):
    model = StaffPerson


class StaffPersonCreateView(CreateView):
    model = StaffPerson


class StaffPersonUpdateView(UpdateView):
    model = StaffPerson
