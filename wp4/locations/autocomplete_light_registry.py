# from django import http
#
# import autocomplete_light.shortcuts as al
#
# from ..staff_person.models import StaffPerson
# from .models import Hospital
#
#
# class HospitalAutoComplete(al.AutocompleteModelBase):
#     choices = Hospital.objects.filter(is_active=True)
#     search_fields = ['name', 'country']
#     attrs = {
#         # This will set the input placeholder attribute:
#         'placeholder': 'Please type in the hospital name',
#         # This will set the yourlabs.Autocomplete.minimumCharacters
#         # options, the naming conversion is handled by jQuery
#         'data-autocomplete-minimum-characters': 1,
#     }
#     widget_attrs = {
#         'data-widget-maximum-values': 1,
#         'data-widget-bootstrap': 'hospital-widget',
#     }
#
#     def autocomplete_html(self):
#         html = super(HospitalAutoComplete, self).autocomplete_html()
#         html += '<span data-value="create"><i class="glyphicon glyphicon-save"></i> Save New Hospital</span>'
#         return html
#
#     def post(self, request, *args, **kwargs):
#         current_person = StaffPerson.objects.get(user__id=request.user.id)
#         current_country = current_person.based_at.country
#         new_hospital = Hospital.objects.create(
#             name=request.POST['name'],
#             country=current_country,
#             created_by=current_person.user
#         )
#         return http.HttpResponse(
#             new_hospital.pk
#         )
#
# al.register(Hospital, HospitalAutoComplete)
#
#
# // Autocomplete-light extra widgets - JAVASCRIPT!!
# HospitalWidget = {
#     getValue: function (choice) {
#         // TODO: Secure this input so that it doesn't execute the contents of value!
#         var value = choice.data('value');
#
#         if (value == 'create') {
#             choice.html(this.input.val())
#
#             $.ajax(this.autocomplete.url, {
#                 async: false,
#                 type: 'post',
#                 data: {
#                     'name': this.input.val(),
#                 },
#                 success: function (text, jqXHR, textStatus) {
#                     value = text;
#                 }
#             });
#
#             choice.data('value', value);
#         }
#
#         return value;
#     }
# }