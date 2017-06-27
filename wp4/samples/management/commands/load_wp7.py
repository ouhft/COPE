#!/usr/bin/python
# coding: utf-8
from django.core.management.base import LabelCommand
from django.utils import timezone

from wp4.samples.utils import WP7Workbook, load_wp7_xlsx


class Command(LabelCommand):
    help = "Load a specified WP7 Export provided spreadsheet of data"

    def handle_label(self, label, **options):
        # Generate some stats for feedback
        start_time = timezone.now()

        if label.split(".")[-1] != "xlsx":
            raise Exception("This is not an xlsx file")
        print("load_wp7: Workbook ({0}) is about to load".format(label))

        # Load the xlsx file with the raw data
        workbook = WP7Workbook()
        if not workbook.load_xlsx(label):
            raise Exception("xlsx file failed to load")

        total_rows, created_count, linked_count = load_wp7_xlsx(workbook)

        end_time = timezone.now()
        print(
            "Import completed: Started: {0:%Y-%m-%d %H:%M:%S}. ".format(start_time) +
            "Finished: {0:%Y-%m-%d %H:%M:%S}.".format(end_time) +
            "Added: {0}. Linked: {1}. Total: {2}".format(created_count, linked_count, total_rows)
        )
