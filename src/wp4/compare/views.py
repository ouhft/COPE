from django.http import HttpResponse, Http404
from django.views.generic.edit import CreateView, UpdateView, DeleteView
from django.core.urlresolvers import reverse_lazy

from django.shortcuts import get_object_or_404, render
from django.http import HttpResponseRedirect
from django.core.urlresolvers import reverse
from django.views import generic

from .models import RetrievalTeam, Hospital, Person

class PersonIndexView(generic.ListView):
    template_name = 'person/index.html'
    context_object_name = 'people_list'

    def get_queryset(self):
        return Person.objects.order_by('-created_on')


class PersonDetailView(generic.DetailView):
    model = Person
    template_name = 'person/detail.html'


class PersonResultsView(generic.DetailView):
    model = Person
    template_name = 'person/results.html'


class PersonCreate(CreateView):
    model = Person
    template_name = 'person/form.html'
    fields = ['first_names', 'last_names', 'job', 'telephone', 'user']

    def form_valid(self, form):
        form.instance.created_by = self.request.user
        return super(PersonCreate, self).form_valid(form)


class PersonUpdate(UpdateView):
    model = Person
    template_name = 'person/form.html'
    fields = ['first_names', 'last_names', 'job', 'telephone', 'user']


class PersonDelete(DeleteView):
    model = Person
    success_url = reverse_lazy('person_index')


def error404(request):
    raise Http404("This is a page holder")

def DashboardIndex(request):
    return render(request, 'dashboard/index.html', {})



# TESTING STUFF BELOW
class RetrievalTeamIndexView(generic.ListView):
    template_name = 'teams/index.html'
    context_object_name = 'latest_team_list'

    def get_queryset(self):
        """Return the last five published teams."""
        return RetrievalTeam.objects.order_by('-created_on')[:5]


class RetrievalTeamDetailView(generic.DetailView):
    model = RetrievalTeam
    template_name = 'teams/detail.html'


class RetrievalTeamResultsView(generic.DetailView):
    model = RetrievalTeam
    template_name = 'teams/results.html'


def vote(request, question_id):
    raise Http404("This is a I Can't Be Arsed Error")
#     p = get_object_or_404(Question, pk=question_id)
#     try:
#         selected_choice = p.choice_set.get(pk=request.POST['choice'])
#     except (KeyError, Choice.DoesNotExist):
#         # Redisplay the question voting form.
#         return render(request, 'polls/detail.html', {
#             'question': p,
#             'error_message': "You didn't select a choice.",
#         })
#     else:
#         selected_choice.votes += 1
#         selected_choice.save()
#         # Always return an HttpResponseRedirect after successfully dealing
#         # with POST data. This prevents data from being posted twice if a
#         # user hits the Back button.
#         return HttpResponseRedirect(reverse('polls:results', args=(p.id,)))
