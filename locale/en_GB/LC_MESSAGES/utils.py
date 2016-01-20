
# Scan the language file to extract Labels for Questions
# Excludes Constants, Validations, Metadata, Forms, Template blocks, etc...

import re
lines = [line.rstrip('\n') for line in open('/Users/carl/Projects/cope/cope_repo/locale/en_GB/LC_MESSAGES/django.po')]
clean_lines = []
row = {'location': '', 'label': '', 'display': ''}
for line in lines:
    if line[0:2] == "#:":
        if "models" in line and "followups" not in line and "adverse" not in line:
            row["location"] = line[3:]
        else:
            row["location"] = ''
    elif line[0:5] == "msgid":
        row["label"] = ''
        p_constants = re.compile('[A-Z]{2}c[0-9]{2}')
        p_validation = re.compile('[A-Z]{2}v[0-9]{2}')
        p_metadata = re.compile('[A-Z]{2}m[0-9]')
        if p_constants.search(line) is not None or p_validation.search(line) is not None or p_metadata.search(line) is not None:
            continue
        row["label"] = line[7:-1]
    elif line[0:6] == "msgstr":
        row["display"] = line[8:-1]
    elif line == '' and row["location"] != '' and row["label"] != '':
        clean_lines.append(row.copy())

for line in clean_lines:
    print('"%s", "%s", "%s"' % (line.get('location'), line.get('label'), line.get('display')))
